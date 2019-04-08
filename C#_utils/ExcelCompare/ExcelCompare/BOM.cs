using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExcelCompare
{

    public class Item
    {
        /// <summary>
        /// 子件编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 子件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 子件规格
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 基本用量
        /// </summary>
        public decimal Count { get; set; }
        /// <summary>
        /// 区位
        /// </summary>
        public string Position { get; set; }


        public override string ToString()
        {
            return string.Format("子件编码-{0},子件名称-{1},子件规格-{2},基本用量-{3},区位-{4},备注-{5}", Code, Name, Mode, Count, Position, Remark);
        }
    }

    /// <summary>
    /// 按区位分析
    /// </summary>
    public class ItemByPosition
    {
        /// <summary>
        /// 子件编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 子件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 子件规格
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 旧BOM备注
        /// </summary>
        public string OldRemark { get; set; }
        /// <summary>
        /// 来自旧BOM
        /// </summary>
        public bool IsOld { get; set; }
        /// <summary>
        /// 来自新BOM
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 新BOM备注
        /// </summary>
        public string NewRemark { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                string result = "--";
                result = IsOld ? "不焊" : "焊";
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("子件编码-{0},子件名称-{1},子件规格-{2},更改前备注-{3},更改后备注-{4},结果-{5}",
                Code, Name, Mode, OldRemark, NewRemark, Description);
        }
    }


    /// <summary>
    /// 按区位分析
    /// </summary>
    public class ItemByPositionOrderBy
    {
        /// <summary>
        /// 区号
        /// </summary>
        public string Position { get; set; }
        /// <summary>
        /// 子件编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 子件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 子件规格
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 旧BOM备注
        /// </summary>
        public string OldRemark { get; set; }
        /// <summary>
        /// 来自旧BOM
        /// </summary>
        public bool IsOld { get; set; }
        /// <summary>
        /// 来自新BOM
        /// </summary>
        public bool IsNew { get; set; }
        /// <summary>
        /// 新BOM备注
        /// </summary>
        public string NewRemark { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get
            {
                string result = "--";
                result = IsOld ? "不焊" : "焊";
                return result;
            }
        }

        public override string ToString()
        {
            return string.Format("子件编码-{0},子件名称-{1},子件规格-{2},更改前备注-{3},更改后备注-{4},结果-{5}",
                Code, Name, Mode, OldRemark, NewRemark, Description);
        }
    }
    /// <summary>
    /// 按子件分析
    /// </summary>
    public class ItemByItem
    {
        /// <summary>
        /// 子件编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 子件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 子件规格
        /// </summary>
        public string Mode { get; set; }
        private string _OldRemark { get; set; }
        /// <summary>
        /// 旧BOM备注
        /// </summary>
        public string OldRemark
        {
            get { return _OldRemark; }
            set
            {
                if (string.IsNullOrEmpty(_OldRemark))
                {
                    _OldRemark = value.Trim();
                }
            }
        }
        /// <summary>
        /// 来自旧BOM
        /// </summary>
        public bool IsOld { get; set; }
        /// <summary>
        /// 来自新BOM
        /// </summary>
        public bool IsNew { get; set; }
        private string _NewRemark { get; set; }
        /// <summary>
        /// 新BOM备注
        /// </summary>
        public string NewRemark
        {
            get { return _NewRemark; }
            set
            {
                if (string.IsNullOrEmpty(_NewRemark))
                {
                    _NewRemark = value.Trim();
                }
            }
        }

        /// <summary>
        /// 改前数量
        /// </summary>
        public decimal OldCount { get; set; }
        /// <summary>
        /// 改后数量
        /// </summary>
        public decimal NewCount { get; set; }

        /// <summary>
        /// 改前区位
        /// </summary>
        public string OldPosition { get; set; }
        /// <summary>
        /// 改后区位
        /// </summary>
        public string NewPosition { get; set; }

        public override string ToString()
        {
            return string.Format("子件编码-{0},子件名称-{1},子件规格-{2},备注-{3}/{4},数量-{5}/{6},区位-{7}/{8}",
                Code, Name, Mode, OldRemark, NewRemark, OldCount, NewCount, OldPosition, NewPosition);
        }
    }


    public class ItemByItemOrderBy
    {

        /// <summary>
        /// 子件编码
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// 子件名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 子件规格
        /// </summary>
        public string Mode { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 改后数量
        /// </summary>
        public decimal NewCount { get; set; }
        /// <summary>
        /// 改前数量
        /// </summary>
        public decimal OldCount { get; set; }

        public bool Compare
        {
            get
            {
                return OldCount - NewCount>0;
            }
        }
        public override string ToString()
        {
            return string.Format("子件编码-{0},子件名称-{1},子件规格-{2},备注-{3},数量-{4}/{5}",
                Code, Name, Mode, Remark, OldCount, NewCount);
        }
    }

}
