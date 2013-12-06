using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace npo_comp_contextMenu
{
    class Item
    {
        string _name = "";
        // список синонимов к name
        public List<string> listSynonims = new List<string>();
        int _level = 0;
        int _defaultPosition = 0;
        string _parent = "";
        int _colClicks = 0;
        //является ли пункт контекстного меню избранным (по умолчанию "нет")
        public bool favorit = false;
        public bool enabled = false;
        //наименование пункта контекстного меню
        public string name 
        {
            get { return _name;}
            set { name = _name;}
        }
        //уровень иерархии пункта контекстного меню, на котором он находится (0 - невложенный уровень)
        public int level
        {
            get { return _level; }
            set { level = _level;}
        }
        //позиция в списке на своём уровне иерархии (на уровне level)
        public int defaultPosition
        {
            get { return _defaultPosition; }
            set { defaultPosition = _defaultPosition;}
        }
        //уровен, непосредственно предществующий данному (для parent name является вложенным) (родитель)
        public string parent
        {
            get { return _parent; }
            set { parent = _parent;}
        }
        //количество кликов по данному пункту контекстного меню
        public int colClicks
        {
            get { return _colClicks;}
            set { colClicks = _colClicks;}
        }
        //задает имя пункта конктесного меню
        public void setName(string s)
        {
            _name = s;
        }
        //сбрасывает имя пункта контекстного меню
        public void resetName()
        {
            _name = "";
        }
        //задает уровень иерархии пункта контекстного меню
        public void setLevel(int i)
        {
            _level = i;
        }
        //сбрасывает уровень иерархии пункта контекстного меню до значения по умолчанию (0)
        public void resetLevel()
        {
            _level = 0;
        }
        //задает позицию пункта контекстного меню в списке на своём уровне иерархии (на уровне level)
        public void setDefaultPosition(int i)
        {
            _defaultPosition = i;
        }
        //сбрасывает позицию пункта контестного меню до первой строки на уровне иерархии (индекс первой строки = 0)
        public void resetDefaultPosition()
        {
            _defaultPosition = 0;
        }
        //задает родителя пункта контекстного меню
        public void setParent(string s)
        {
            _parent = s;
        }
        //делает пункт контекстного меню не вложенным 
        public void resetParent()
        {
            _parent = "";
        }
        //задает количество кликов по пункту контекстного меню
        public void setKolClicks(int i)
        {
            _colClicks = i;
        }
        //обнуляет количество кликов
        public void resetAllClicks()
        {
            _colClicks = 0;
        }
        //инкрементирует количество кликов
        public void incClicks()
        {
            _colClicks++;
        }
    }
}
