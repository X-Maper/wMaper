using System.Collections.Generic;
using System.Linq;
using WMagic;
using WMaper.Core;

namespace WMaper.Meta.Store
{
    /// <summary>
    /// 地图符号
    /// </summary>
    public sealed class Symbol
    {
        #region 变量

        // 基础图层集合
        private Dictionary<string, Geog> tile;
        // 专题图层集合
        private Dictionary<string, Geog> pile;
        // 注记图层集合
        private Dictionary<string, Note> note;
        // 绘图图层集合
        private Dictionary<string, Geom> draw;
        // 地标图层集合
        private Dictionary<string, Mark> mark;
        // 提示图层集合
        private Dictionary<string, Tips> tips;

        #endregion

        #region 属性

        public Dictionary<string, Geog> Tile
        {
            get { return this.tile; }
            set { this.tile = value; }
        }

        public Dictionary<string, Geog> Pile
        {
            get { return this.pile; }
            set { this.pile = value; }
        }

        public Dictionary<string, Note> Note
        {
            get { return this.note; }
            set { this.note = value; }
        }

        public Dictionary<string, Geom> Draw
        {
            get { return this.draw; }
            set { this.draw = value; }
        }

        public Dictionary<string, Mark> Mark
        {
            get { return this.mark; }
            set { this.mark = value; }
        }

        public Dictionary<string, Tips> Tips
        {
            get { return this.tips; }
            set { this.tips = value; }
        }

        #endregion

        #region 构造函数

        public Symbol()
        {
            this.tile = new Dictionary<string, Geog>();
            this.pile = new Dictionary<string, Geog>();
            this.note = new Dictionary<string, Note>();
            this.draw = new Dictionary<string, Geom>();
            this.mark = new Dictionary<string, Mark>();
            this.tips = new Dictionary<string, Tips>();
        }

        #endregion

        #region 函数方法

        public void Dispose()
        {
            if (!MatchUtils.IsEmpty(this.tile))
            {
                Geog[] a_tile = this.tile.Values.ToArray();
                for (int i = a_tile.Length - 1; i > -1; i--)
                {
                    try
                    {
                        a_tile[i].Remove();
                    }
                    catch
                    {
                        continue;
                    }
                    finally
                    {
                        a_tile[i] = null;
                    }
                }
                // 释放内存
                this.tile.Clear();
                {
                    this.tile = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.pile))
            {
                Geog[] a_pile = this.pile.Values.ToArray();
                for (int i = a_pile.Length - 1; i > -1; i--)
                {
                    try
                    {
                        a_pile[i].Remove();
                    }
                    catch
                    {
                        continue;
                    }
                    finally
                    {
                        a_pile[i] = null;
                    }
                }
                // 释放内存
                this.pile.Clear();
                {
                    this.pile = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.note))
            {
                Note[] a_note = this.note.Values.ToArray();
                for (int i = a_note.Length - 1; i > -1; i--)
                {
                    try
                    {
                        a_note[i].Remove();
                    }
                    catch
                    {
                        continue;
                    }
                    finally
                    {
                        a_note[i] = null;
                    }
                }
                // 释放内存
                this.note.Clear();
                {
                    this.note = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.draw))
            {
                Geom[] a_draw = this.draw.Values.ToArray();
                for (int i = a_draw.Length - 1; i > -1; i--)
                {
                    try
                    {
                        a_draw[i].Remove();
                    }
                    catch
                    {
                        continue;
                    }
                    finally
                    {
                        a_draw[i] = null;
                    }
                }
                // 释放内存
                this.draw.Clear();
                {
                    this.draw = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.mark))
            {
                Mark[] a_mark = this.mark.Values.ToArray();
                for (int i = a_mark.Length - 1; i > -1; i--)
                {
                    try
                    {
                        a_mark[i].Remove();
                    }
                    catch
                    {
                        continue;
                    }
                    finally
                    {
                        a_mark[i] = null;
                    }
                }
                // 释放内存
                this.mark.Clear();
                {
                    this.mark = null;
                }
            }
            if (!MatchUtils.IsEmpty(this.tips))
            {
                Tips[] a_tips = this.tips.Values.ToArray();
                for (int i = a_tips.Length - 1; i > -1; i--)
                {
                    try
                    {
                        a_tips[i].Remove();
                    }
                    catch
                    {
                        continue;
                    }
                    finally
                    {
                        a_tips[i] = null;
                    }
                }
                // 释放内存
                this.tips.Clear();
                {
                    this.tips = null;
                }
            }
        }

        #endregion
    }
}