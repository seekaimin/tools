﻿using System;
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
    public partial class FormByPosition : Form
    {
        public FormByPosition()
        {
            InitializeComponent();
        }
        string oldpath = "";
        string newpath = "";
        string oldenclosurepath = "";
        string newenclosurepath = "";
        void SelectFile(TextBox txt)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    txt.Text = ofd.FileName;
                }
            }
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
            oldenclosurepath = "";
            newenclosurepath = "";
            try
            {
                new Thread(() =>
                {
                    try
                    {
                        Change();
                        oldpath = txtold.GetText();
                        newpath = txtnew.GetText();
                        oldenclosurepath = txtoldfj.GetText();
                        newenclosurepath = txtnewfj.GetText();
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
            string ofj = txtoldfj.Text.Trim();
            string n = txtnew.Text.Trim();
            string nfj = txtnewfj.Text.Trim();
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
            Tools.SetConfig(Tools.OLD_ENCLOSURE_PATHNAME, ofj);
            Tools.SetConfig(Tools.NEW_ENCLOSURE_PATHNAME, nfj);
            Tools.Save();
            List<Item> obom = ExcelFileHelper.readFile(o, ShowConsole);
            if (ofj.Length > 0)
            {
                List<Item> t = ExcelFileHelper.readFile(ofj, ShowConsole);
                obom.AddRange(t);
            }
            List<Item> nbom = ExcelFileHelper.readFile(n, ShowConsole);
            if (nfj.Length > 0)
            {
                List<Item> t = ExcelFileHelper.readFile(nfj, ShowConsole);
                nbom.AddRange(t);
            }
            ShowConsole("文件分析完成  开始比较...");

            Dictionary<string, List<ItemByPosition>> items = compare(obom, nbom);

            List<ItemByPositionOrderBy> showitems = new List<ItemByPositionOrderBy>();
            foreach (string key in items.Keys)
            {
                if (items[key].Count > 0)
                {
                    foreach (ItemByPosition i in items[key])
                    {
                        ItemByPositionOrderBy item = new ItemByPositionOrderBy()
                        {
                            Code = i.Code,
                            Name = i.Name,
                            Mode = i.Mode,
                            Position = key,
                            IsNew = i.IsNew,
                            IsOld = i.IsOld,
                            NewRemark = i.NewRemark,
                            OldRemark = i.OldRemark
                        };
                        showitems.Add(item);
                    }
                }
            }



            DGV.DoAction(() =>
            {
                var tempitems = showitems.OrderByDescending(p => p.Code).ThenBy(p => p.IsNew);
                foreach (ItemByPositionOrderBy i in tempitems)
                {
                    ShowConsole("子件 : {0}", i.ToString());
                    if (i.IsOld != i.IsNew)
                    {
                        int index = DGV.Rows.Add();
                        DGV.Rows[index].HeaderCell.Value = (index + 1).ToString();
                        DGV.Rows[index].Cells[colPosition.Name].Value = i.Position;
                        if (i.IsOld)
                        {
                            DGV.Rows[index].Cells[colOldCode.Name].Value = i.Code;
                            DGV.Rows[index].Cells[colOldName.Name].Value = i.Name;
                            DGV.Rows[index].Cells[colOldMode.Name].Value = i.Mode;
                            DGV.Rows[index].Cells[colOldRemark.Name].Value = i.OldRemark;


                            DGV.Rows[index].Cells[colNewCode.Name].Value = "无";

                        }
                        if (i.IsNew)
                        {
                            DGV.Rows[index].Cells[colNewCode.Name].Value = i.Code;
                            DGV.Rows[index].Cells[colNewName.Name].Value = i.Name;
                            DGV.Rows[index].Cells[colNewMode.Name].Value = i.Mode;
                            DGV.Rows[index].Cells[colNewRemark.Name].Value = i.NewRemark;


                            DGV.Rows[index].Cells[colOldCode.Name].Value = "无";
                        }
                        DGV.Rows[index].Cells[colDescription.Name].Value = i.IsNew ? "焊" : "不焊";
                    }
                }
            });
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
            txtoldfj.Text = Tools.GetConfig(Tools.OLD_ENCLOSURE_PATHNAME);
            txtnewfj.Text = Tools.GetConfig(Tools.NEW_ENCLOSURE_PATHNAME);


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
                t.Cursor = System.Windows.Forms.Cursors.Arrow;  //指定鼠标形状（更好看）  
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

        private void btnslect_old_clear_Click(object sender, EventArgs e)
        {
            txtold.Text = "";
        }

        private void btnslect_old_fj_clear_Click(object sender, EventArgs e)
        {
            txtoldfj.Text = "";
        }

        private void btnslect_new_clear_Click(object sender, EventArgs e)
        {
            txtnew.Text = "";
        }

        private void btnslect_new_fj_clear_Click(object sender, EventArgs e)
        {
            txtnewfj.Text = "";
        }

        private void txtSelect_DoubleClick(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == null) { return; }
            SelectFile(txt);
        }



        Dictionary<string, List<ItemByPosition>> compare(List<Item> oldbom, List<Item> newbom)
        {
            Dictionary<string, List<ItemByPosition>> result = new Dictionary<string, List<ItemByPosition>>();
            #region new
            foreach (Item item in newbom)
            {
                string[] positions = item.Position.Split(' ', '\t', '.', '\n', '\r');
                foreach (string position in positions)
                {
                    string pos = position.Trim();
                    if (pos.Length == 0 || pos.Equals("见附件"))
                    {
                        continue;
                    }
                    if (!result.ContainsKey(pos))
                    {
                        //不存在
                        result.Add(pos, new List<ItemByPosition>());
                    }
                    ItemByPosition temp = result[pos].FirstOrDefault(p => p.Code == item.Code);
                    if (temp == null)
                    {
                        temp = new ItemByPosition()
                        {
                            Code = item.Code,
                            Mode = item.Mode,
                            Name = item.Name,
                            OldRemark = "",
                            NewRemark = item.Remark,
                            IsOld = false,
                            IsNew = true,
                        };
                        result[pos].Add(temp);
                    }
                    else
                    {
                        temp.IsNew = true;
                        temp.NewRemark = item.Remark;
                    }
                }
            }
            #endregion
            #region old
            foreach (Item item in oldbom)
            {
                string[] positions = item.Position.Split(' ', '\t', '.', '\n', '\r');
                foreach (string position in positions)
                {
                    string pos = position.Trim();
                    if (pos.Length == 0 || pos.Equals("见附件"))
                    {
                        continue;
                    }
                    if (!result.ContainsKey(pos))
                    {
                        //不存在
                        result.Add(pos, new List<ItemByPosition>());
                    }
                    ItemByPosition temp = result[pos].FirstOrDefault(p => p.Code == item.Code);
                    if (temp == null)
                    {
                        temp = new ItemByPosition()
                        {
                            Code = item.Code,
                            Mode = item.Mode,
                            Name = item.Name,
                            OldRemark = item.Remark,
                            NewRemark = "",
                            IsOld = true,
                            IsNew = false,
                        };
                        result[pos].Add(temp);
                    }
                    else
                    {
                        temp.IsOld = true;
                        temp.OldRemark = item.Remark;
                    }
                }
            }
            #endregion
            return result;
        }
    }
}