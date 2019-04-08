using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Util.WinForm.ExtensionHelper;
using System.Threading;
using Util.WinForm.DGVHelpers;

namespace ExcelCompare
{
    public partial class FormByItem : Form
    {
        public FormByItem()
        {
            InitializeComponent();
        }
        string oldpath = "";
        string newpath = "";
        private void btnslect_old_Click(object sender, EventArgs e)
        {
            txtold.Text = "";
        }

        private void btnslect_new_Click(object sender, EventArgs e)
        {
            txtnew.Text = "";
        }

        public bool running
        {
            get
            {
                return btnCompare.Enabled == false;
            }
            set
            {
                btnCompare.SetPropertry<bool>("Enabled", !value);
            }
        }
        private void btnCompare_Click(object sender, EventArgs e)
        {
            if (running) { return; }
            running = true;
            txtConsole.Clear();
            DGV.Rows.Clear();
            oldpath = "";
            newpath = "";
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        Change();
                        oldpath = txtold.GetText();
                        newpath = txtnew.GetText();
                    }
                    catch (Exception ex)
                    {
                        ShowConsole(ex.Message);
                    }
                    finally
                    {
                        running = false;
                        ShowConsole("分析完成...");
                    }
                }).Start();
            }
            catch (Exception ex)
            {
                ShowConsole(ex.Message);
                running = false;
            }
        }
        void Change()
        {
            ShowConsole("分析开始...");
            string o = txtold.Text.Trim();
            string n = txtnew.Text.Trim();
            ShowConsole("旧BOM文件 : {0}", o);
            ShowConsole("新BOM文件 : {0}", n);
            if (o.Length == 0)
            {
                throw new Exception("请先选择旧BOM文件!");
            }
            if (n.Trim().Length == 0)
            {
                throw new Exception("请先选择新BOM文件!");
            }
            //保存配置
            Tools.SetConfig(Tools.OLD_PATHNAME, o);
            Tools.SetConfig(Tools.NEW_PATHNAME, n);
            Tools.Save();
            List<Item> bomold = ExcelFileHelper.readFile(o, ShowConsole);
            List<Item> bomnew = ExcelFileHelper.readFile(n, ShowConsole);
            ShowConsole("文件分析完成  开始比较...");


            //key:子件编码
            Dictionary<string, List<ItemByItem>> items = compare(bomold, bomnew);

            List<ItemByItemOrderBy> showitems = new List<ItemByItemOrderBy>();

            #region 封装需要显示子件信息
            foreach (string key in items.Keys)
            {
                foreach (ItemByItem i in items[key])
                {
                    if ((i.IsOld != i.IsNew) || (i.NewCount != i.OldCount))
                    {
                        ItemByItemOrderBy temp = new ItemByItemOrderBy();
                        temp.Code = i.Code;
                        temp.Name = i.Name;
                        temp.Mode = i.Mode;
                        temp.OldCount = i.OldCount;
                        temp.NewCount = i.NewCount;
                        string remark = "";
                        if (temp.NewCount > 0)
                        {
                            remark = i.NewRemark;
                        }
                        else
                        {
                            remark = i.NewRemark;
                            if (string.IsNullOrEmpty(remark))
                            {
                                remark = i.OldRemark;
                            }
                        }
                        temp.Remark = remark;
                        showitems.Add(temp);
                    }
                }
            }
            #endregion
            var tempitems = showitems.OrderByDescending(p => p.Compare).ThenByDescending(p => p.Code);

            DGV.DoAction(() =>
            {
                foreach (ItemByItemOrderBy i in tempitems)
                {
                    ShowConsole("区位 : {0}", i.Code);
                    ShowConsole("子件 : {0}", i.ToString());
                    int index = DGV.Rows.Add();
                    DGV.Rows[index].HeaderCell.Value = (index + 1).ToString();

                    DGV.Rows[index].Cells[colCode.Name].Value = i.Code;
                    DGV.Rows[index].Cells[colName.Name].Value = i.Name;
                    DGV.Rows[index].Cells[colMode.Name].Value = i.Mode;

                    DGV.Rows[index].Cells[colOldCount.Name].Value = i.OldCount;
                    DGV.Rows[index].Cells[colNewCount.Name].Value = i.NewCount;
                    DGV.Rows[index].Cells[colRemark.Name].Value = i.Remark;
                }
            });
        }
        Dictionary<string, List<ItemByItem>> compare(List<Item> oldbom, List<Item> newbom)
        {
            Dictionary<string, List<ItemByItem>> result = new Dictionary<string, List<ItemByItem>>();
            #region new
            foreach (Item item in newbom)
            {
                if (!result.ContainsKey(item.Code))
                {
                    result.Add(item.Code, new List<ItemByItem>());
                }
                ItemByItem temp = result[item.Code].FirstOrDefault(p => p.Code == item.Code);
                if (temp == null)
                {
                    temp = new ItemByItem()
                    {
                        Code = item.Code,
                        Mode = item.Mode,
                        Name = item.Name,
                        IsOld = false,
                        IsNew = true,
                        OldRemark = "",
                        NewRemark = item.Remark,
                        OldCount = 0,
                        NewCount = item.Count,
                        OldPosition = "",
                        NewPosition = item.Position,
                    };
                    result[item.Code].Add(temp);
                }
                else
                {
                    temp.IsNew = true;
                    temp.NewRemark = item.Remark;
                    temp.NewCount = item.Count;
                    temp.NewPosition = item.Position;
                }
            }
            #endregion
            #region old
            foreach (Item item in oldbom)
            {
                if (!result.ContainsKey(item.Code))
                {
                    //不存在
                    result.Add(item.Code, new List<ItemByItem>());
                }
                ItemByItem temp = result[item.Code].FirstOrDefault(p => p.Code == item.Code);
                if (temp == null)
                {
                    temp = new ItemByItem()
                    {
                        Code = item.Code,
                        Mode = item.Mode,
                        Name = item.Name,
                        IsOld = true,
                        IsNew = false,
                        OldRemark = item.Remark,
                        NewRemark = "",
                        OldCount = item.Count,
                        NewCount = 0,
                        OldPosition = item.Position,
                        NewPosition = "",
                    };
                    result[item.Code].Add(temp);
                }
                else
                {
                    temp.IsOld = true;
                    temp.OldRemark = item.Remark;
                    temp.OldCount = item.Count;
                    temp.OldPosition = item.Position;
                }
            }
            #endregion
            return result;
        }

        void ShowConsole(string format, params object[] p)
        {
            ShowConsole(string.Format(format, p));
        }
        void ShowConsole(string msg)
        {
            txtConsole.DoAction(() =>
            {
                Tools.d(msg);
                txtConsole.AppendText(msg);
                txtConsole.AppendText("\r\n");
            });
        }

        private void txtBox_DragDrop(object sender, DragEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (t == null) { return; }
            string path = ((System.Array)e.Data.GetData(DataFormats.FileDrop)).GetValue(0).ToString();
            t.Text = path;
            t.Cursor = System.Windows.Forms.Cursors.IBeam; //还原鼠标形状

        }
        private void Form1_Load(object sender, EventArgs e)
        {
            txtold.AllowDrop = txtnew.AllowDrop = true;
            txtold.Text = Tools.GetConfig(Tools.OLD_PATHNAME);
            txtnew.Text = Tools.GetConfig(Tools.NEW_PATHNAME);
            foreach (DataGridViewColumn column in DGV.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }
        private void txtBox_DragEnter(object sender, DragEventArgs e)
        {
            TextBox t = sender as TextBox;
            if (t == null) { return; }
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Link;
                t.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
            else
            {
                e.Effect = DragDropEffects.None;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            if (oldpath.Length == 0) { return; }

            int start = oldpath.LastIndexOf('\\');
            int end = oldpath.LastIndexOf('.');
            string oldname = oldpath.Substring(start + 1, end - start - 1);

            start = newpath.LastIndexOf('\\');
            end = newpath.LastIndexOf('.');
            string newname = newpath.Substring(start + 1, end - start - 1);
            string savefile = string.Format("{0}_to_{1}.csv", oldname, newname);
            Console.WriteLine("保存的文件名称   {0}", savefile);
            DGV.Export(savefile);
        }

        private void txtnew_DoubleClick(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) { return; }
            Tools.SelectFile(txt);
        }
    }
}