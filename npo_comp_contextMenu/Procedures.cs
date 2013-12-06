using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;

namespace npo_comp_contextMenu
{
    class Procedures
    {
        public Bitmap CreateBitmap(Color color)
        {
            // Генерируем изображение в виде цветного прямоугольника
            Bitmap bm = new Bitmap(16, 16);
            Graphics gr = Graphics.FromImage(bm);
            gr.FillRectangle(new SolidBrush(color), 0, 0, 16, 16);
            gr.Dispose();   // Освобождаем контекст устройства
            return bm;
        }

        public string InsertSpaceOut(string str)
        {
            // Вставляем пробелы перед заглавными буквами названия цвета
            for (int i = str.Length - 1; i > 1; i--)
                if (Char.IsUpper(str[i]))
                    str = str.Insert(i, " ");
            return str;
        }

        //добавление вложенного иерархического списка (count - количество пунктов списка, idx - индекс пункта списка в массиве всех пунктов, к которому нужно добавить вложение,
        //l - список всех пунктов вложения, newItem добавляемый пункт меню с вложениями)
        public ToolStripMenuItem setIList(int count, string parent, List<Item> l)
        {

            ToolStripItem[] dropDownItems = new ToolStripItem[count];
            ContextMenuStrip cms = new ContextMenuStrip();
            for (int j = 0; j < count; j++)
            {
                cms.Items.Add(l[j].name);
                dropDownItems[j] = cms.Items[j];
            }
            ToolStripMenuItem newItem = new ToolStripMenuItem(parent, null, dropDownItems);
            return newItem;
        }

        //возвращает список только тех индексов, которые указаны в intList
        public List<Item> setDropDownList(List<int> intList, List<Item> itemList)
        {
            List<Item> l = new List<Item>();
            for (int j = 0; j < intList.Count; j++)
                for (int i = 0; i < itemList.Count; i++)
                {
                    //if (s.IndexOf(i.ToString()) != -1)
                    if (intList[j] == i)
                        l.Add(itemList[i]);
                }
            return l;
        }

        public //делает все пункты меню доступными или недоступными
        void AllEnabled(ContextMenuStrip cms, bool ok)
        {
            string type = "";
            foreach (ToolStripItem item in cms.Items)
            {
                type = item.GetType().ToString();
                if (type != "System.Windows.Forms.ToolStripTextBox")
                    item.Enabled = ok;
            }
            for (int k = 1; k < cms.Items.Count; k++)
            {
                type = cms.Items[k].GetType().ToString();
                if (type != "System.Windows.Forms.ToolStripSeparator")
                    for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)
                    {
                        type = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                        if (type != "System.Windows.Forms.ToolStripSeparator")
                            ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].Enabled = ok;
                    }
            }
            for (int k = 1; k < cms.Items.Count; k++)
            {
                type = cms.Items[k].GetType().ToString();
                if (type != "System.Windows.Forms.ToolStripSeparator")
                    for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)
                    {
                        type = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                        if (type != "System.Windows.Forms.ToolStripSeparator")
                            for (int m = 0; m < ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.Count; m++)
                                ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].Enabled = ok;
                    }
            }
            for (int k = 1; k < cms.Items.Count; k++)
            {
                type = cms.Items[k].GetType().ToString();
                if (type != "System.Windows.Forms.ToolStripSeparator")
                    for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)
                    {
                        type = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                        if (type != "System.Windows.Forms.ToolStripSeparator")
                            for (int m = 0; m < ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.Count; m++)
                            {
                                type = ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].GetType().ToString();
                                if (type != "System.Windows.Forms.ToolStripSeparator")
                                    for (int n = 0; n < ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems.Count; n++)
                                        ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n].Enabled = ok;
                            }
                    }
            }
            for (int k = 1; k < cms.Items.Count; k++)
            {
                type = cms.Items[k].GetType().ToString();
                if (type != "System.Windows.Forms.ToolStripSeparator")
                    for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)
                    {
                        type = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                        if (type != "System.Windows.Forms.ToolStripSeparator")
                            for (int m = 0; m < ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.Count; m++)
                            {
                                type = ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].GetType().ToString();
                                if (type != "System.Windows.Forms.ToolStripSeparator")
                                    for (int n = 0; n < ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems.Count; n++)
                                    {
                                        type = ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n].GetType().ToString();
                                        if (type != "System.Windows.Forms.ToolStripSeparator")
                                            for (int o = 0; o < ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n]).DropDownItems.Count; o++)
                                                ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n]).DropDownItems[o].Enabled = ok;
                                    }
                            }
                    }
            }
            cms.Update();
        }

        

        //задает у всех предков данного пунка Enabled = true 
        public void setAllParentsEnabled(ToolStripItem item, int level)
        {
            switch (level)
            {
                case 0: { }
                    break;
                case 1: {
                    ToolStripItem itemParent = item.OwnerItem;
                    itemParent.Enabled = true;
                }
                    break;
                case 2: {
                    ToolStripItem itemParent = item.OwnerItem;
                    ToolStripItem itemGrantParent = itemParent.OwnerItem;
                    itemGrantParent.Enabled = true;
                    itemParent.Enabled = true;
                }
                    break;
                case 3: {
                    ToolStripItem itemParent = item.OwnerItem;
                    ToolStripItem itemGrantParent = itemParent.OwnerItem;
                    ToolStripItem itemGrantGrantParent = itemGrantParent.OwnerItem;
                    itemGrantGrantParent.Enabled = true;
                    itemGrantParent.Enabled = true;
                    itemParent.Enabled = true;
                
                }
                    break;
                case 4: {
                    ToolStripItem itemParent = item.OwnerItem;
                    ToolStripItem itemGrantParent = itemParent.OwnerItem;
                    ToolStripItem itemGrantGrantParent = itemGrantParent.OwnerItem;
                    ToolStripItem itemGrantGrantGrantParent = itemGrantGrantParent.OwnerItem;
                    itemGrantGrantGrantParent.Enabled = true;
                    itemGrantGrantParent.Enabled = true;
                    itemGrantParent.Enabled = true;
                    itemParent.Enabled = true;
                }
                    break;
            }
        
        }

        

        public void setItemVar34(Item it, string request, string levelLength, ContextMenuStrip cms)
        {
            string space = "";
            string name = it.name;
            switch (it.level)
            {
                case 0: space = "";
                    break;
                case 1: space = levelLength;
                    break;
                case 2: space = levelLength + levelLength;
                    break;
                case 3: space = levelLength + levelLength + levelLength;
                    break;
                case 4: space = levelLength + levelLength + levelLength + levelLength;
                    break;
            }
            ToolStripMenuItem item = new ToolStripMenuItem(space + name);
            item.ToolTipText = "Чтобы добавить в избранное, кликните правой кнопкой";
            string s = name.ToLower();
            if (s.IndexOf(request) != -1)
                item.Enabled = true;
            else
                item.Enabled = false;
            cms.Items.Add(item);
        }

        public string setLine(Item it, /*string request, */string levelLength)
        {
            string result = "";
            string space = "";
            switch (it.level)
            {
                case 0: space = "";
                    break;
                case 1: space = levelLength;
                    break;
                case 2: space = levelLength + levelLength;
                    break;
                case 3: space = levelLength + levelLength + levelLength;
                    break;
                case 4: space = levelLength + levelLength + levelLength + levelLength;
                    break;
            }
            result = space + it.name;
            return result;
        }




    }
}
