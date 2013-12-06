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
using System.Resources;


//ContextMenuStrip поддерживает события Opening и Closing с возможностью отмены, 
//чтобы обрабатывать динамическое заполнение и ситуации с несколькими щелчками.

//узнать по какому элементу щелкнули 
//ToolStripMenuItem tlm = (sender) as ToolStripMenuItem;
//            MessageBox.Show(tlm.Name);

//Появление флажка слева от команды включается булевым свойством Checked объекта команды. В качестве альтернативы можно установить свойство CheckState в одно из значений одноименного перечисления. Например для объекта itemOpen это можно определить так

//itemOpen.CheckState = CheckState.Indeterminate ;
//itemOpen.CheckState = CheckState.Checked ;
//itemOpen.CheckState = CheckState.Unchecked ;
//Опция Indeterminate позволяет отобразить неопределенное состояние, когда на месте флажка помещается точка.


// Добавляем в коллекцию команды Font
//ToolStripMenuItem item = 
//    (ToolStripMenuItem)itemFont.DropDownItems.Add(font.Name);
    
//// Если встретился текущий шрифт формы, 
//// запоминаем эту команду и отмечаем флажком
//if (font.Name == this.Font.Name)
//{
//    itemSelectedFont = item;
//    itemSelectedFont.Checked = true;
//}



namespace npo_comp_contextMenu
{
    public partial class Form1 : Form
    {
        // Поле для сохранения выбранного элемента меню
        ToolStripMenuItem itemChecked = new ToolStripMenuItem();
        // Применяем рефлексию к структуре, представляющей цвета
        PropertyInfo[] api = typeof(Color).GetProperties();
        //в этой переменной все функции
        Procedures proc = new Procedures();
        public Form1()
        {
            InitializeComponent();
            
            

            contextMenu.ShowCheckMargin = false;
            currentVariant.Text = "Вариант 1";

            /*для быстрого редактирования меню*/

            // списком доступных шрифтов стиля "Обычный"
            Graphics gr = CreateGraphics(); // Ссылка на контекст устройства
            // Читаем все шрифты 
            FontFamily[] fonts = FontFamily.GetFamilies(gr);
            // Перебираем шрифты и формируем команды меню Font
            foreach (FontFamily font in fonts)
            {
                // Выбираем шрифты одного стиля "Обычный"
                if (font.IsStyleAvailable(FontStyle.Regular))
                {
                    // Добавляем в коллекцию команды Font
                    comboBoxFont.Items.Add(font.Name);
                    if (font.Name == this.Font.Name)
                    {
                        comboBoxFont.Text = "Segoe UI";
                    }

                }
            }
            textBoxFontSize.Text = "9";
            textBoxItemHeight.Text = "15";
            textBoxItemWidth.Text = this.richTextBoxSearch.Size.Width.ToString();
            textBoxOtstup.Text = "5";
            //textBoxTopLeftAnglePanel.Text = "3;2";
            //textBoxTopLeftAnglePanel.Text = "3;4";
            for (int i = 0; i < 100; i++)
            {
                string sss = "";
                for (int j = 0; j < 100; j++)
                    sss += j.ToString();
                listBoxDynamic.Items.Add(sss);
            }
            maxWidthSearch = labelItem1.Size.Width;
            maxWidthCMS = lbl.Size.Width;
            maxHeightCMS = lbl.Size.Height;
            //присваиваем стандартные цвета контекстному меню
            myContextMenu.BackColor = colorDefault;
            panelSearch.BackColor = colorDefault;

            oldlabelLocationX = labelItem1.Location.X;
            oldLabelLocationY = labelItem1.Location.Y;

            oldDeleteLabelLocationX = labelDeleteItem1.Location.X;
            oldDeleteLabelLocationY = labelDeleteItem1.Location.Y;
            //richTextBoxSearch.BackColor = colorDefault;

            panelSearchSize = new Size(217, 38);
            topLeftFirstItemPanelSearch = new Point(firstItemPanelSearch.Location.X, firstItemPanelSearch.Location.Y);
            defaultPanelSearchSize = new Size(panelSearch.Size.Width, topLeftFirstItemPanelSearch.Y);
            //defaultMyCMSSize = new Size(myContextMenu.Size.Width, topLeftFirstItemPanelSearch.Y);
            defaultMyMenuSize = new Size(myContextMenu.Size.Width, myContextMenu.Size.Height);
            //panelSearch.Controls.Add(panelFind);
        }
        //загружены ли цвета
        bool colorsWasDownloaded = false;
        
        //список переменных и необходимых параметров
        int mouseX = 0;
        int mouseY = 0;
        string levelLenghtVar1 = "      ";
        string levelLenghtVar2 = "      ";
        string levelLenghtVar3 = "   ";
        string levelLenghtVar4 = "      ";
        string levelLenghtVar5 = "      ";
        string levelLenghtVar6 = "      ";
        //была ли нажата правая клавиша мыши (для добавления в избранное)
        bool mouseRightButtonClick = false;
        //показаны избранные пункты
        bool favoritesShow = false;
        //загружено ли новое меню
        bool newOpen = false;
        string filenameFavorit = Application.StartupPath+"\\img\\Pushpin.bmp";
        string filenameFavoritSelect = Application.StartupPath + "\\img\\Pushpin_select.bmp";
        string filenameFavoritFar = Application.StartupPath + "\\img\\Pushpin_far.bmp";
        string filenameFavoritFarSelect = Application.StartupPath + "\\img\\Pushpin_far_select.bmp";
        string filenameDelete = Application.StartupPath + "\\img\\Delete.bmp";
        string filenameDeleteSelect = Application.StartupPath + "\\img\\Delete_select.bmp";
        string filenameDeleteFar = Application.StartupPath + "\\img\\Delete_far.bmp";
        string filenameDeleteFarSelect = Application.StartupPath + "\\img\\Delete_far_select.bmp";
        string filenameNone = Application.StartupPath + "\\img\\none.bmp";

        //максимальная видимая ширина метки в окне поиска(варианты 5 и 6)
        int maxWidthSearch = 0; 
        //в окне контекстного меню
        int maxWidthCMS = 0;
        int maxHeightCMS = 0;
        int maxWidthCMSlevel1 = 0;
        int maxHeightCMSlevel1 = 0;
        int maxWidthCMSlevel2 = 0;
        int maxHeightCMSlevel2 = 0;
        int maxWidthCMSlevel3 = 0;
        int maxHeightCMSlevel3 = 0;
        int maxWidthCMSlevel4 = 0;
        int maxHeightCMSlevel4 = 0;
        //координаты последнего открытого уровня меню
        Point curentPanelLocation = new Point();
        //активная панель
        Panel curentPanel = new Panel();
        //умолчания
        int defaultWidthCMS = 0;
        int defaultHeightCMS = 0;
        int defaultWidthSearch = 0;
        int defaultHeightSearch = 0;
        Size defaultPanelSearchSize = new Size(0, 0);
        Size defaultMyCMSSize = new Size(0, 0);
        Size defaultMyMenuSize = new Size(0, 0);
        Size defaultPanelSize = new Size(0, 0);
        Size panelSearchSize = new Size(0, 0);
        string fontText = "Segoe UI";
        Point topLeftAnglePanel = new Point(0, 0);
        Point topLeftAngleLabel = new Point(0, 0);
        Point topLeftFirstItemPanelSearch = new Point(0,0);
        Font font = new System.Drawing.Font("Segoe UI", (float)9, FontStyle.Regular);
        // координаты левого верхнего угла кликнутой иконки
        int currentClickItemX = 0;
        int currentClickItemY = 0;
        //цвет выделения пункта меню при наведении мыши
        Color colorWhenMouseHover = Color.LightBlue;
        Color colorBorder = Color.Blue;
        Color colorClick = Color.Yellow;
        //стандартный цвет контекстного меню и окна поиска
        Color colorDefault = Color.WhiteSmoke;
        //для выделения предков, не соответствующих запросу
        Color colorNotEnabled = Color.LightGray;
        // Цвет основного текста
        Color colorText = Color.Black;
        //для добавления новых меток на панель
        int oldlabelLocationX = 0;
        int oldLabelLocationY = 0;
        int oldDeleteLabelLocationX = 0;
        int oldDeleteLabelLocationY = 0;
        //настройки
        int panelIerarchCMSOtstup = 0;
        //ничего не найдено
        bool nothingFound = false;
        //максимальное значение любого из списков иерархических групп
        int maxValueList = 100;
        //настройки по умолчанию
        void defaultTools()
        {
            string filename = Application.StartupPath + "\\#tools.txt";
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("myFont:" + "Segoe UI");
            sw.WriteLine("fontSize:" + "9");
            //
            sw.WriteLine("myCMSItemWidth:" +"196");
            sw.WriteLine("myCMSItemHeight:" + "23");
            sw.WriteLine("myOtstup:"+"5");
            //стандартное количество символов в пункте меню
            sw.WriteLine("maxKolChar:"+"20");
            //ширина пункта
            sw.WriteLine("maxWidthMyContextMenu:" + "200");
            //отступы панели в иерархическом меню
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.WriteLine("");
            sw.Close();
        }
        //количество выводимых часто используемых результатов
        int fastResult 
        {
            get { return Convert.ToInt32(textBoxFastResult.Text); }
        }
        //если кликнули по пункту меньше этого количества раз, он выводиться не будет
        int kolCliks 
        {
            get { return Convert.ToInt32(textBoxKolClicks.Text); }
        }
        //максимальное количество символов в пунтке меню
        int maxKolChar
        {
            get { return Convert.ToInt32(textBoxMaxKolChar.Text); }
        }
        //ширина буквы
        int maxWidthMyContextMenu
        {
            get { return Convert.ToInt32(textBoxCharSize.Text); }
        }
        Form2 form;
        //содержит все пункты меню
        static List<Item> listItems = new List<Item>();
        //считает количество кликов по каждому пункту меню
        static List<int> listItemsClicks = new List<int>();
        //чтобы загрузить сразу несколько файлов с контекстными менюшками
        List<List<Item>> listAllVariants = new List<List<Item>>();
        List<string> listListDynamic = new List<string>();
        List<Item> allFavorites = new List<Item>();
        //начальное состояние меню
        ToolStripItemCollection defaultCollection;
        //были ли загружены пункты меню
        bool wasDownload = false;
        //лог ошибок
        List<string> logErorrs = new List<string>();
        //лог загрузок
        List<string> logDownloads = new List<string>();
        //лог других сведений
        List<string> logOther = new List<string>();
        //для лога
        string sepLog = "# ";
        void clearContextMenu(ContextMenuStrip cMenu)
        {
            //textBox не затираем
            if (cMenu.Name == "contextMenu")
                for (int i = 1; i < cMenu.Items.Count; i++)
                {
                    cMenu.Items.RemoveAt(i);
                    i--;
                }
        }

        string myFont {
            get { return comboBoxFont.Text; }
        }

        double myFontSize {
            get {
                return Convert.ToDouble(textBoxFontSize.Text);
            }
        }

        int myCMSItemHeight 
        {
            get {
                return Convert.ToInt32(textBoxItemHeight.Text);
            }
        }

        int myCMSItemWidth
        {
            get
            {
                return Convert.ToInt32(textBoxItemWidth.Text);
            }
        }

        int myOtstup
        {
            get
            {
                return Convert.ToInt32(textBoxOtstup.Text);
            }
        }

        void setAllColor()
        {

            // Перебор цветов
            /* 14 секунд делает цвета*/
            foreach (PropertyInfo pi in api)
            {
                if (pi.CanRead && pi.PropertyType == typeof(Color)
                    && pi.Name != "Transparent")
                {
                    // Извлекаем очередной цвет
                    Color color = (Color)pi.GetValue(null, null);
                    // Создаем очередной элемент подменю
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    // По букве цвета определяем подменю, в котором разместим цвет
                    string typeItem = "";
                    for (int k = 0; k < menu.Items.Count; k++)//0
                    {
                        typeItem = (menu.Items[k]).GetType().ToString();
                        if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                            typeItem != "System.Windows.Forms.ToolStripTextBox")
                        {
                            if (menu.Items[k].Text == "Настройки")
                            {
                                if (((ToolStripMenuItem)menu.Items[k]).DropDownItems.Count != 0)
                                    //перебираем вложения первого уровня
                                    for (int l = 0; l < ((ToolStripMenuItem)menu.Items[k]).DropDownItems.Count; l++)//1
                                    {
                                        typeItem = ((ToolStripMenuItem)menu.Items[k]).DropDownItems[l].GetType().ToString();
                                        if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                            typeItem != "System.Windows.Forms.ToolStripTextBox")
                                        {
                                            if (((ToolStripMenuItem)menu.Items[k]).DropDownItems[l].Text == "Цвета выделений")
                                                //перебираем вложения второго уровня
                                                for (int m = 0; m < ((ToolStripMenuItem)((ToolStripMenuItem)menu.Items[k]).DropDownItems[l]).DropDownItems.Count; m++)//2
                                                {
                                                    typeItem = ((ToolStripMenuItem)((ToolStripMenuItem)menu.Items[k]).DropDownItems[l]).DropDownItems[m].GetType().ToString();
                                                    if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                                        typeItem != "System.Windows.Forms.ToolStripTextBox")
                                                    {
                                                        // Извлекаем очередной цвет
                                                        Color c = (Color)pi.GetValue(null, null);
                                                        // Создаем очередной элемент подменю
                                                        ToolStripMenuItem it = new ToolStripMenuItem();
                                                        ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)menu.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems.Add(it);
                                                        // Продолжаем настраивать команду
                                                        it.Text = proc.InsertSpaceOut(c.Name);// Пробелы перед заглавными буквами цвета
                                                        it.Name = c.Name;
                                                        // Продолжаем настраивать команду
                                                        it.Text = proc.InsertSpaceOut(color.Name);// Пробелы перед заглавными буквами цвета
                                                        it.Name = color.Name;// Сохраняем название цвета для обработчика ColorOnClick
                                                        it.Image = proc.CreateBitmap(color);// Тонкой ссылке присваиваем толстый объект
                                                        it.Click += new EventHandler(ColorOnClick);// Регистрация обработчика
                                                        if (color.Equals(this.BackColor))// Отметить флажком текущий цвет и сохранить ссылку
                                                            (itemChecked = it).Checked = true;
                                                    }
                                                }
                                        }
                                    }
                            }
                        }

                    }


                }
            }
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
            MyOpen();
            
            //a();
        }
        //сохраняет все данные о пунктах в лог
        void saveAllItems()
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                string s = (i + 1).ToString() + ". Наименование пункта:" + listItems[i].name + ";\n" +
                    "Уровень иерархии: " + listItems[i].level.ToString() + ";\n" +
                    "Позиция на уровне иерархии: " + listItems[i].defaultPosition.ToString() + ";\n" +
                    "Родитель пункта: " + listItems[i].parent + ";\n" +
                    "Количество кликов по пункту: " + listItems[i].colClicks.ToString() + ";\n";
                s += "Является ли пункт избранным: ";
                if (listItems[i].favorit)
                    s += "Да;\n";
                s += "Нет;\n";
                for (int j = 0; j < listItems[i].listSynonims.Count; j++)
                {
                    if (j == listItems[i].listSynonims.Count - 1)
                        s += listItems[i].listSynonims[j] + ".";
                    else
                        s += listItems[i].listSynonims[j] + ", ";
                }
                logDownloads.Add(s);
            }
        }

        string myShortFileName = "";
        void getShortFileName(string s)
        {
            char[] c = {'\\'};
            string[] ss = s.Split(c);
            myShortFileName = "Загружено из файла: " + ss[ss.Length - 1];
            Text = Text + ". " + myShortFileName;
        }

        void MyOpen()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Текстовые файлы (*.TXT, *.DOC, *.DOCX)|*.txt; *.doc; *docx";
            dialog.InitialDirectory = Application.StartupPath + "\\txt";
            dialog.FileName = "Новый текстовый документ.txt";
            dialog.DefaultExt = ".txt";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    clearMyPanel(myContextMenu, 0);
                    clearMyPanel(panelSearch, 2);

                    listAllVariants.Clear();
                    listItems.Clear();
                    listListDynamic.Clear();
                    clearContextMenu(contextMenu);
                    panelSearch.Visible = false;
                    myContextMenu.Visible = false;
                    setLog("Загрузка данных из файла: " + dialog.FileName);
                    //Text += Text + " файл: " + dialog.FileName;
                    getShortFileName(dialog.FileName);
                    ReadFile(dialog.FileName);
                    setLog("Загружается описание пунктов контекстного меню...");
                    saveAllItems();
                    wasDownload = true;
                    setLog("Наполнение контекстного меню");
                    allFavorites.Clear();
                    newOpen = true;
                    //if (currentVariant.Text != "Вариант 5" && currentVariant.Text != "Вариант 6")
                    setContextMenu(contextMenu);
                    //else
                    //setContextMenu(contextMenuRequest);
                    clearMyPanel(myContextMenu, 0);
                    setMyContextMenu();
                    defaultMyCMSSize = new Size(myContextMenu.Size.Width, myContextMenu.Size.Height);
                    defaultWidthCMS = myContextMenu.Size.Width;
                    defaultHeightCMS = myContextMenu.Size.Height;
                    defaultWidthSearch = panelSearch.Size.Width;
                    defaultHeightSearch = panelSearch.Size.Height;
                    textBoxFileName.Text = "Сейчас используется :" + currentVariant.Text + "; Загружено из файла: " + myShortFileName;
                    //сохранили нефильтрованное меню
                    defaultCollection = contextMenu.Items;
                    //поисковая строка должна быть видна всегда
                    contextMenu.Items[0].Enabled = true;
                    contextMenu.Items[0].Text = "";
                    maxWidthSearch = 0;
                    maxWidthCMS = 0;
                    maxHeightCMS = 0;
                    maxWidthCMSlevel1 = 0;
                    maxHeightCMSlevel1 = 0;
                    maxWidthCMSlevel2 = 0;
                    maxHeightCMSlevel2 = 0;
                    maxWidthCMSlevel3 = 0;
                    maxHeightCMSlevel3 = 0;
                    maxWidthCMSlevel4 = 0;
                    maxHeightCMSlevel4 = 0;
                    
                }
                catch (Exception ex)
                {
                    // вывод отдельно строки, где произошло исключение
                    var st = new System.Diagnostics.StackTrace(ex, true);
                    var frame = st.GetFrame(0);
                    int line = frame.GetFileLineNumber();
                    MessageBox.Show("Error: " + ex.Message + "\n" + ex.StackTrace, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    logErorrs.Add("Error: " + ex.Message + "\n" + ex.StackTrace);
                }
            }
        }
        
        //считывает из файла контекстное меню и список listBoxDynamic
        void ReadFile(string filename)
        {
            StreamReader line =
            new StreamReader(filename);
            //считываем контекстное меню
            string name = "";
            string synonim = "";
            //отступ
            string tab = "";
            string dTab = "  ";
            //отступ на предыдущем уровне
            string oldTab = "#";
            //была ли запятая
            bool wasComma = false;
            int level = 0;
            //чтобы сохранять на всех уровнях вложенности количество строк в списках (первый список - для нулевого уровня, остальные - для ветвей)
            List<int>[] listdefaultPosition = new List<int>[5];
            List<int> defaultPosition = new List<int>();
            for (int i = 0; i < 5; i++)
            {
                defaultPosition.Add(0);
                listdefaultPosition[i] = defaultPosition;
            }
            string parent = "";
            bool favorit = false;
            string sBig = line.ReadToEnd();
            string s = "";
            string subs = "";
            bool end = false;
            for (int j = 0; j < sBig.Length; j++)
            {
                if (j < sBig.Length)
                    subs = sBig[j].ToString();
                //отлавливаем переходы на новую строку
                if (subs != "\r")
                {
                    //накапливаем слово
                    while (j < sBig.Length && subs != "\r")
                    {
                        subs = sBig[j].ToString();
                        if (subs != "\r")
                        {
                            s += sBig[j];
                            j++;
                        }
                    }
                    Item item = new Item();
                    tab = "";
                    for (int i = 0; i < s.Length; i++)
                    {
                        while (i < s.Length && !wasComma && name == "")
                        {
                            if (s[i] == ' ' || s[i] == '\n')
                                tab += s[i];
                            else 
                                break;
                            i++;
                        }
                        //наименование пункта заканчивается первой запятой или концом строки
                        if (!wasComma && i < s.Length)
                        {
                            if (s[i] != '\n' && s[i] != '\r')
                            {
                                if (s[i] != ',' && s[i] != '*')
                                    name += s[i];
                                else if (s[i] == '*')
                                {
                                    favorit = true;
                                }
                                else if (s[i] == ',')
                                {
                                    item.setName(name);
                                    wasComma = true;
                                    if (i + 1 < s.Length)
                                        i++;
                                }
                            }
                        }
                        else
                        {
                            //накапливаем синонимы
                            while (i < s.Length && i + 1 < s.Length)
                            {
                                if (s[i] != ',' && s[i] != '*' && s[i] != ' ')
                                {
                                    synonim += s[i];
                                    if (i + 1 < s.Length)
                                        i++;
                                    if (i < s.Length && i + 1 >= s.Length && s[i] != ',' && s[i] != '*' && s[i] != ' ')
                                        synonim += s[i];
                                }
                                else
                                {
                                    if (synonim != "")
                                    {
                                        item.listSynonims.Add(synonim);
                                        synonim = "";
                                        break;
                                    }
                                    else
                                    {
                                        i++;
                                    }
                                }
                            }
                            if (i < s.Length)
                                if (s[i] == '*')
                                {
                                    favorit = true;
                                }
                                else if (s[i] != ',')
                                {
                                    if (synonim != "")
                                    {
                                        item.listSynonims.Add(synonim);
                                    }
                                    synonim = "";
                                }
                        }
                        if (j >= sBig.Length)
                            end = true;
                    }
                    
                    if (oldTab != "#")
                    {
                        tab = tab.Substring(1);
                        //если текущий отступ длинне предыдущего, то текущий пункт - вложен в предыдущий
                        if (tab.Length > oldTab.Length)
                        {
                            level++;
                            //для нового списка новый счетчик строк
                            defaultPosition[level]= 0;
                            
                            //считываем имя предка текущего пункта (последняя запись в список пунктов меню)
                            parent = listItems[listItems.Count - 1].name;
                        }
                        //если текущий отступ короче, то текущий пункт выше иерархией, чем предыдущий
                        else if (tab.Length < oldTab.Length)
                        {
                            switch (tab.Length)
                            {
                                case 0: { level = 0; }
                                    break;
                                case 2: { level = 1; }
                                    break;
                                case 4: { level = 2; }
                                    break;
                                case 6: { level = 3; }
                                    break;
                                case 8: { level = 4; }
                                    break;
                                default: { level = 0; }
                                    break;
                            }
                            defaultPosition[level]++;
                            for (int ii = 0; ii < listItems.Count; ii++)
                            {
                                if (listItems[ii].level < level)
                                    parent = listItems[ii].name;
                                else if (listItems[ii].level == level) 
                                    parent = listItems[ii].parent;
                            }
                        }
                        else if (tab.Length == oldTab.Length)
                        {
                            defaultPosition[level]++;
                            //т.к. пункты находятся в одном списке, отец одинаковый
                            int idx = listItems.Count - 1;
                            try
                            {
                                int kol = 0;
                                while (parent == "" && idx > 0 && kol < listItems.Count)
                                    if (listItems[idx].level == level)
                                    {
                                        parent = listItems[idx].parent;
                                        kol++;
                                    }
                                    else if (level - listItems[idx].level == 1)
                                    {
                                        parent = listItems[idx].name;
                                    }
                                    else if (level - listItems[idx].level > 1)
                                        idx--;
                                    else
                                        break;
                            }
                            catch (Exception e)
                            {
                                MessageBox.Show("Ошибка в назначении родителя. Уровень: " + level.ToString() + ". Наименование пункта: " + name + ". " + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                logErorrs.Add("Ошибка в назначении родителя. Уровень: " + level.ToString() + ". Наименование пункта: " + name + ". " + e.Message);
                            }
                        }
                        
                    }
                    //текущий отступ становится старым отступом
                    oldTab = tab;
                    s = "";
                    //задаем все параменты пункта меню
                    if (name.IndexOf("/n") == -1 && name.Trim() != "" )
                    {
                        item.setName(name);
                        item.setLevel(level);
                        item.setDefaultPosition(defaultPosition[level]);
                        item.setParent(parent);
                        item.favorit = favorit;
                        //записываем его в списков всех пунктов
                        listItems.Add(item);
                        //по умолчанию при загрузке ни один пункт меню не был нажат
                        listItemsClicks.Add(0);
                        listdefaultPosition[level] = defaultPosition;
                        //новую ветвь (позиции) в новый список
                        if (level + 1 == 1)
                            
                            for (int i = 1; i < defaultPosition.Count; i++ )
                                defaultPosition[i] = 0;
                    }
                    name = "";
                    wasComma = false;
                    parent = "";
                    favorit = false;
                    s = "";
                }
                else
                {
                    subs = "";
                }
                //спец символ для обозначения того, что далее ещё возможно непустые строки
                //s = "#@";
            }
            listAllVariants.Add(listItems);
            line.Close();
        }

        //наполняет контекстное меню считанными в ReadFile пунктами
        void setContextMenu(ContextMenuStrip cms)
        {
            try
            {
                List<List<Item>> list = new List<List<Item>>();
                //списки для каждого уровня иерархии
                List<Item> l1 = new List<Item>();
                List<Item> l2 = new List<Item>();
                List<Item> l3 = new List<Item>();
                List<Item> l4 = new List<Item>();
                List<Item> l5 = new List<Item>();
                for (int idx = 0; idx < listItems.Count; idx++)
                {

                    switch (listItems[idx].level)
                    {
                        case 0: l1.Add(listItems[idx]);
                            break;
                        case 1: l2.Add(listItems[idx]);
                            break;
                        case 2: l3.Add(listItems[idx]);
                            break;
                        case 3: l4.Add(listItems[idx]);
                            break;
                        case 4: l5.Add(listItems[idx]);
                            break;
                        default: l1.Add(listItems[idx]);
                            break;
                    }
                }
                list.Add(l1);
                list.Add(l2);
                list.Add(l3);
                list.Add(l4);
                list.Add(l5);

                //список индексов пунктов на определенном этаже иерархии, которые являются вложенными в их общего предка
                List<int> idxList = new List<int>();
                //с самого последнего вложения будем обходить дерево
                int i = list.Count - 1;
                //предыдущий предок
                string oldParent = "";
                List<ToolStripMenuItem> listTSMI = new List<ToolStripMenuItem>();
                bool begin = true;
                bool wasIdxRemove = false;
                while (i >= 0)
                {
                    if (i < list.Count)
                        for (int j = 0; j < list[i].Count; j++)
                        {
                            if (list[i][j].level == 0)
                            {
                                ToolStripMenuItem newItem = new ToolStripMenuItem(list[i][j].name);
                                listTSMI.Add(newItem);
                            }
                            else if (begin)
                            {
                                idxList.Add(j);
                                begin = false;
                            }
                            else if (oldParent == list[i][j].parent)
                            {
                                idxList.Add(j);
                                if (j + 1 >= list[i].Count)
                                {
                                    ToolStripMenuItem newItem = proc.setIList(idxList.Count, list[i][j].parent, proc.setDropDownList(idxList, list[i]));
                                    listTSMI.Add(newItem);
                                    idxList.Clear();
                                    begin = true;
                                }
                            }
                            //oldParent хранит родителя добавленных индексов пунктов меню list[i] 
                            else if (oldParent != list[i][j].parent)
                            {
                                ToolStripMenuItem newItem = proc.setIList(idxList.Count, oldParent, proc.setDropDownList(idxList, list[i]));
                                listTSMI.Add(newItem); //заворачивается на 11
                                idxList.Clear();
                                idxList.Add(j);
                                begin = false;
                                //для случая последнего элемента списка (в противном случае i инкрементируется и происходит противоречие в индексах)
                                if (j + 1 >= list[i].Count)
                                {
                                    newItem = proc.setIList(idxList.Count, list[i][j].parent, proc.setDropDownList(idxList, list[i]));
                                    listTSMI.Add(newItem);
                                    idxList.Clear();
                                    begin = true;
                                }
                            }
                            oldParent = list[i][j].parent;
                        }
                    i--;
                }

                List<ToolStripItem[]> dropDownItems = new List<ToolStripItem[]>();
                List<ToolStripMenuItem> listI = new List<ToolStripMenuItem>();

                for (int j = 0; j < listTSMI.Count; j++)
                {
                    //узнаем количество вложенных пунктов
                    int count = listTSMI[j].DropDownItems.Count;
                    if (count != 0)
                    {
                        //узнаем наименование пункта
                        string text = listTSMI[j].Text;
                        //создаем хранилице вложенных пунктов
                        ToolStripItem[] ddi = new ToolStripItem[count];
                        //записываем туда вложенные пункты
                        for (int k = 0; k < count; k++)
                            ddi[k] = listTSMI[j].DropDownItems[k];
                        dropDownItems.Add(ddi);
                        ToolStripMenuItem item = new ToolStripMenuItem(text, null, ddi[count - 1]);
                        //item.Click += new EventHandler(item_Click);
                        item.MouseEnter += new EventHandler(item_MouseEnter);
                        listI.Add(item);
                    }
                    else
                    {
                        if (listTSMI[j].Text != "-")
                        {
                            listTSMI[j].DropDownItemClicked += new ToolStripItemClickedEventHandler(item_DropDownItemClicked);
                            listTSMI[j].MouseEnter += new EventHandler(item_MouseEnter);
                            cms.Items.Add(listTSMI[j]);
                        }
                        else
                            cms.Items.Add("-");
                    }
                }
                ToolStripItemCollection collection = cms.Items;
                //первый уровень меню заполнили, теперь заполняем вложениями
                int countList = listI.Count;
                string typeItem = "";
                //заполняем первый уровень 
                for (int j = 0; j < listI.Count; j++)
                {
                    //перебираем вложения нулевого уровня
                    for (int k = 0; k < cms.Items.Count; k++)
                    {
                        typeItem = cms.Items[k].GetType().ToString();
                        if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                            typeItem != "System.Windows.Forms.ToolStripTextBox")
                        {
                            if (listI[j].Text == cms.Items[k].Text)
                            {
                                ToolStripMenuItem item = new ToolStripMenuItem(listI[j].Text, null, dropDownItems[j]);
                                cms.Items.RemoveAt(k);
                                cms.Items.Insert(k, item);
                                item.Text = listI[j].Text;
                                if (listI[j].Text != "-")
                                {
                                    item.DropDownItemClicked += new ToolStripItemClickedEventHandler(item_DropDownItemClicked);
                                    item.MouseEnter += new EventHandler(item_MouseEnter);
                                }
                                countList--;
                            }
                        }
                    }
                }

                //заполняем второй уровень
                for (int j = 0; j < listI.Count; j++)
                {
                    //перебираем вложения нулевого уровня
                    for (int k = 0; k < cms.Items.Count; k++)//0
                    {
                        typeItem = (cms.Items[k]).GetType().ToString();
                        if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                            typeItem != "System.Windows.Forms.ToolStripTextBox")
                        {
                            if (((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count != 0)
                                //перебираем вложения первого уровня
                                for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)//1
                                {
                                    typeItem = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                                    if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                        typeItem != "System.Windows.Forms.ToolStripTextBox")
                                    {
                                        if (listI[j].Text == ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].Text)
                                        {
                                            ToolStripMenuItem item = new ToolStripMenuItem(listI[j].Text, null, dropDownItems[j]);
                                            ((ToolStripMenuItem)cms.Items[k]).DropDownItems.RemoveAt(l);
                                            ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Insert(l, item);
                                            item.Text = listI[j].Text;
                                            if (listI[j].Text != "-")
                                                item.DropDownItemClicked += new ToolStripItemClickedEventHandler(item_DropDownItemClicked);
                                            countList--;
                                        }
                                    }
                                }
                        }
                    }
                }

                //заполняем третий уровень
                for (int j = 0; j < listI.Count; j++)
                {
                    //перебираем вложения нулевого уровня
                    for (int k = 0; k < cms.Items.Count; k++)//0
                    {
                        typeItem = (cms.Items[k]).GetType().ToString();
                        if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                            typeItem != "System.Windows.Forms.ToolStripTextBox")
                        {
                            if (((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count != 0)
                                //перебираем вложения первого уровня
                                for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)//1
                                {
                                    typeItem = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                                    if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                        typeItem != "System.Windows.Forms.ToolStripTextBox")
                                    {
                                        //перебираем вложения второго уровня
                                        for (int m = 0; m < ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.Count; m++)//2
                                        {
                                            typeItem = ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].GetType().ToString();
                                            if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                                typeItem != "System.Windows.Forms.ToolStripTextBox")
                                                if (listI[j].Text == ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].Text)
                                                {
                                                    ToolStripMenuItem item = new ToolStripMenuItem(listI[j].Text, null, dropDownItems[j]);
                                                    ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.RemoveAt(m);
                                                    ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.Insert(m, item);
                                                    item.Text = listI[j].Text;
                                                    if (listI[j].Text != "-")
                                                        item.DropDownItemClicked += new ToolStripItemClickedEventHandler(item_DropDownItemClicked);
                                                    countList--;
                                                }
                                        }
                                    }

                                }
                        }
                    }
                }

                //заполняем четвертый уровень
                for (int j = 0; j < listI.Count; j++)
                {
                    //перебираем вложения нулевого уровня
                    for (int k = 0; k < cms.Items.Count; k++)//0
                    {
                        typeItem = (cms.Items[k]).GetType().ToString();
                        if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                            typeItem != "System.Windows.Forms.ToolStripTextBox")
                        {
                            if (((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count != 0)
                                //перебираем вложения первого уровня
                                for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)//1
                                {
                                    typeItem = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                                    if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                        typeItem != "System.Windows.Forms.ToolStripTextBox")
                                    {
                                        //перебираем вложения второго уровня
                                        for (int m = 0; m < ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.Count; m++)//2
                                        {
                                            typeItem = ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].GetType().ToString();
                                            if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                                typeItem != "System.Windows.Forms.ToolStripTextBox")
                                            {
                                                //перебираем вложения третьего уровня
                                                for (int n = 0; n < ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems.Count; n++)//2
                                                {
                                                    typeItem = ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n].GetType().ToString();
                                                    if (typeItem != "System.Windows.Forms.ToolStripSeparator" &&
                                                        typeItem != "System.Windows.Forms.ToolStripTextBox")
                                                        if (listI[j].Text == ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n].Text)
                                                        {
                                                            ToolStripMenuItem item = new ToolStripMenuItem(listI[j].Text, null, dropDownItems[j]);
                                                            ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems.RemoveAt(n);
                                                            ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems.Insert(n, item);
                                                            item.Text = listI[j].Text;
                                                            if (listI[j].Text != "-")
                                                                item.DropDownItemClicked += new ToolStripItemClickedEventHandler(item_DropDownItemClicked);
                                                            countList--;
                                                        }
                                                }
                                            }
                                        }
                                    }
                                }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в функции setContextMenu.\n" + e.StackTrace + "\n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в функции setContextMenu.\n" + e.StackTrace + "\n" + e.Message);
            }
        }

        //создает временный файл для просмотра текущего лога
        private void createTxtFile(string filename)
        {
            try
            {
                StreamWriter file = new StreamWriter(filename);
                file.WriteLine("Время записи:" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                //for (int i = 0; i < logOther.Count; i++)
                //    file.WriteLine(logOther[i]);
                
                for (int i = 0; i < logDownloads.Count; i++)
                    file.WriteLine(logDownloads[i]);

                if (logErorrs.Count == 0)
                    file.WriteLine("Произошедших ошибок не зафиксировано.");
                else
                {
                    file.WriteLine("Произошедшие ошибки:");
                    for (int i = 0; i < logErorrs.Count; i++)
                    {
                        file.WriteLine((i + 1).ToString() + ". " + logErorrs[i]);
                        file.WriteLine("");
                    }
                }
                file.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка, связанная с временным файлом. " + e.StackTrace + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка, связанная с временным файлом. " + e.StackTrace + e.Message);
            }
        }
        //сохраняет все логи в файл
        void saveToTxt(string filename)
        {
            try
            {
                StreamWriter file;
                //для дописывания в конец файла
                FileInfo fi = new FileInfo(filename);
                file = fi.AppendText();
                file.WriteLine("Время записи:" + DateTime.Now.ToString("yyyy.MM.dd HH:mm:ss"));
                for (int i = 0; i < logOther.Count; i++)
                {
                    file.WriteLine(logOther[i]);
                }
                if (logErorrs.Count == 0)
                    file.WriteLine("Произошедших ошибок не зафиксировано.");
                else
                {
                    file.WriteLine("Произошедшие ошибки:");
                    for (int i = 0; i < logErorrs.Count; i++)
                    {
                        file.WriteLine((i + 1).ToString() + ". " + logErorrs[i]);
                        file.WriteLine("");
                    }
                }

                file.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка, связанная с лог-файлом. " + e.StackTrace + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка, связанная с лог-файлом. " + e.StackTrace + e.Message);
            }
        }


        private void curentVariant_TextChanged(object sender, EventArgs e)
        {
            idxFocusetItemDown = 0;

            string s = "Вы используете контекстное меню " + currentVariant.Text + ": ";
            setLog("Смена варианта контекстного меню на: " + currentVariant.Text);
            switch (currentVariant.Text)
            {
                case "Вариант 1": { s += "Обычное меню (развернутые результаты поиска)."; }
                    break;
                case "Вариант 2": { s += "Обычное меню (свернутые результаты поиска)."; }
                    break;
                case "Вариант 3": { s += "Панель частоиспользуемых."; }
                    break;
                case "Вариант 4": { s += "Панель избранного."; }
                    break;
                case "Вариант 5": { s += "Обычное меню + Панель частоиспользуемых."; }
                    break;
                case "Вариант 6": { s += "Обычное меню + Панель избранного."; }
                    break;
            }
            Text = s;
            s = "";
            if (currentVariant.Text == "Вариант 1" || currentVariant.Text == "Вариант 2")
            {
                panelSearch.Visible = false;
                myContextMenu.Visible = false;
                listBoxDynamic.ContextMenuStrip = contextMenu; 
            }
            else
            {
                if (currentVariant.Text == "Вариант 3")
                {
                    panelTextBox.Text = "";
                    getFastItemsForPanel();
                    //setPanelSize(panelSearch, defaultPanelSearchSize.Width, defaultPanelSearchSize.Height);
                    if (panelSearch.Visible)
                        myContextMenu.Visible = false;
                }
                else if (currentVariant.Text == "Вариант 4")
                {
                    panelTextBox.Text = "";
                    getFavoritesForPanel();
                    //setPanelSize(panelSearch, defaultPanelSearchSize.Width, defaultPanelSearchSize.Height);
                    if (panelSearch.Visible)
                        myContextMenu.Visible = false;
                }
                else if (currentVariant.Text == "Вариант 5")
                {
                    panelTextBox.Text = "";
                    if (panelSearch.Visible)
                        myContextMenu.Visible = true;
                    clearMyPanel(myContextMenu, 0);
                    setMyContextMenu();
                    setPanelSize(panelSearch, defaultPanelSearchSize.Width, defaultPanelSearchSize.Height);
                    setPanelSize(myContextMenu, defaultMyCMSSize.Width, defaultMyCMSSize.Height);
                }
                else if (currentVariant.Text == "Вариант 6")
                {
                    panelTextBox.Text = "";
                    if (panelSearch.Visible)
                        myContextMenu.Visible = true;
                    clearMyPanel(myContextMenu, 0);
                    setMyContextMenu();
                    setPanelSize(panelSearch, defaultPanelSearchSize.Width, defaultPanelSearchSize.Height);
                    setPanelSize(myContextMenu, defaultMyCMSSize.Width, defaultMyCMSSize.Height);
                }
                listBoxDynamic.ContextMenuStrip = null;
            }
        }

        void search(string request)
        {
            switch (currentVariant.Text)
            {
                case "Вариант 1": { var1(request, levelLenghtVar1, contextMenu); }
                    break;
                case "Вариант 2": { var2(request, contextMenu); }
                    break;
                case "Вариант 3": { var3(request, levelLenghtVar3, panelSearch); }
                    break;
                case "Вариант 4": { var4(request, levelLenghtVar4, panelSearch); }
                    break;
                case "Вариант 5": { var5(request, levelLenghtVar5, myContextMenu); }
                    break;
                case "Вариант 6": { var6(request, levelLenghtVar6, myContextMenu); }
                    break;
            }
        }

        //выдает по пункту меню, всех его родителей (начиная с самого старого)
        List<Item> getAllParents(Item item)
        {
            List<Item> list = new List<Item>();
            int level = item.level;
            string parent1 = "";
            string parent2 = "";
            string parent3 = "";
            if (level == 0)
            {
                return list;
            }
            else if (level == 1)
            {
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 0)
                        if (listItems[i].name == item.parent)
                            list.Add(listItems[i]);
            }
            else if (level == 2)
            {
                
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 1)
                        if (listItems[i].name == item.parent)
                        {
                            parent1 = listItems[i].parent;
                            list.Add(listItems[i]);
                        }
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 0)
                        if (listItems[i].name == parent1)
                            list.Add(listItems[i]);
            }
            else if (level == 3)
            {
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 2)
                        if (listItems[i].name == item.parent)
                        {
                            parent2 = listItems[i].parent;
                            list.Add(listItems[i]); 
                        }
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 1)
                        if (listItems[i].name == parent2)
                        {
                            parent1 = listItems[i].parent;
                            list.Add(listItems[i]);
                        }
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 0)
                        if (listItems[i].name == parent1)
                            list.Add(listItems[i]);
            }
            else if (level == 4)
            {
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 3)
                        if (listItems[i].name == item.parent)
                        {
                            parent3 = listItems[i].parent;
                            list.Add(listItems[i]);
                        }
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 2)
                        if (listItems[i].name == parent3)
                        {
                            parent2 = listItems[i].parent;
                            list.Add(listItems[i]);
                        }
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 1)
                        if (listItems[i].name == parent2)
                        {
                            parent1 = listItems[i].parent;
                            list.Add(listItems[i]);
                        }
                for (int i = 0; i < listItems.Count; i++)
                    if (listItems[i].level == 0)
                        if (listItems[i].name == parent1)
                            list.Add(listItems[i]);
            }
            List<Item> l = new List<Item>();
            for (int i = 0; i < list.Count; i++)
                l.Add(list[list.Count - 1 - i]);
            return l;
        }

        //для результатов поиска
        class searchResult
        {
            public Item item = new Item();
            public string req = "";
            public string levelLength = "";
            public ContextMenuStrip cms;
            public Panel panel;

            public void setAll(Item i, string r, string l, ContextMenuStrip c, Panel p)
            {
                item = i;
                req = r;
                levelLength = l;
                cms = c;
                panel = p;
            }
        }

        /*||||||||||||||||||ВАРИАНТЫ|||||||||||||||||||||*/
        //для удовлетворяющих запросу пунктов меню
        List<Item> lsr = new List<Item>();
        //консоль + раскрытый иерархический список, выводящийся деревом на нулевом уровне меню
        //если нет запроса - обычное контекстное меню с поиском наверху
        void var1(string request/*запрос*/, string levelLength /*количество пробелов, разделяющих уровни*/, ContextMenuStrip cms /*используемое контекстное меню*/)
        {
            try
            {
                lsr.Clear();
                ToolStripItemCollection c = contextMenu.Items;
                level = -1;
                findRequest(request.ToLower(), c);
                clearContextMenu(cms);
                /*
                //каждый список - подшедшие запросу пункты ветки одного из пунктов нулевого уровня
                List<List<Item>> searchBrunchResult = new List<List<Item>>();
                foreach (Item item in listItems)
                {
                    string s = item.name.ToLower();
                    if (s.IndexOf(request) != -1)
                    {
                        List<Item> allParents = getAllParents(item);
                        for (int i = 0; i < allParents.Count; i++)
                        { 
                            //setItemVar1(allParents[i], request, levelLength, cms);
                            searchResult sr = new searchResult();
                            sr.setAll(allParents[i], request, levelLength, cms, null);
                            lsr.Add(sr);
                        }
                        searchResult srt = new searchResult();
                        srt.setAll(item, request, levelLength, cms, null);
                        lsr.Add(srt);
                        //setItemVar1(item, request, levelLength, cms);
                    }
                }
                */
                //удаляем дублирующие
                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (lsr[i].name.TrimStart().IndexOf(lsr[j].name.TrimStart()) != -1 && lsr[i].parent == lsr[j].parent)
                            {
                                lsr.RemoveAt(j);
                                if (j - 1 > 0)
                                    j--;
                                else
                                    break;
                                if (i - 1 > 0)
                                    i--;
                            }
                        }
                    }
                }

                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (i < j && lsr[i].parent == lsr[j].name)
                            {
                                Item ite = lsr[i];
                                lsr[i] = lsr[j];
                                lsr[j] = ite;
                            }
                        }
                    }
                }
                foreach (Item sr in lsr)
                //foreach (Item sr in listItems)
                    setItemVar1(sr, request, levelLength, cms);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе var1./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе var1./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }
        }

        int level = -1;
        void findRequest(string request, ToolStripItemCollection c)
        {
            level++;
            for (int i = 0; i < c.Count; i++)
            {
                string typeItem = (c[i]).GetType().ToString();
                if (typeItem != "System.Windows.Forms.ToolStripSeparator" && typeItem != "System.Windows.Forms.ToolStripTextBox")
                {
                    string item = c[i].Text.ToLower();
                    //удовлетворяет ли запросу пункт
                    if (item.IndexOf(request) != -1 || c[i].Text.IndexOf(request) != -1)
                    {
                        //для начала найдем анализируемый пункт в списке пунктов
                        for (int j = 0; j < listItems.Count; j++)
                        {
                            string myItem = listItems[j].name.ToLower();
                            if (item.IndexOf(myItem) != -1 && level == listItems[j].level /*|| c[i].Text.IndexOf(listItems[j].name) != -1 && level == listItems[j].level*/)
                                //если уровни и имена совпали идем дальше
                                lsr.Add(listItems[j]);
                        }
                    }
                    //есть ли у пункта дети
                    if (hasChildren(c[i], level))
                    {
                        //удовлетворяющие запросу дети или синонимы
                        if (hasChildrenRequestOk(c[i], level, request))
                        {
                            for (int k = 0; k < listItems.Count; k++)
                            {
                                string myitem = listItems[k].name.ToLower();
                                if (item.IndexOf(myitem) != -1)
                                {
                                    lsr.Add(listItems[k]);
                                }
                            }
                            ToolStripItemCollection collection = ((ToolStripMenuItem)c[i]).DropDownItems;
                            findRequest(request.ToLower(), collection);
                        }
                    }
                    //может синонимы удовлетворяют запросу
                    if (item.IndexOf(request) == -1 /*&& c[i].Text.IndexOf(request) == -1*/)
                    {
                        for (int j = 0; j < listItems.Count; j++)
                        {
                            string myItem = listItems[j].name.ToLower();
                            if (item.IndexOf(myItem) != -1 && level == listItems[j].level /*|| c[i].Text.IndexOf(listItems[j].name) != -1 && level == listItems[j].level*/)
                                if (isSynonimRequestOk(listItems[j], request))
                                    lsr.Add(listItems[j]);
                        }
                    }
                    
                    
                } 
            }
            level--;
        }

        //перебирает список синонимов пункта на совпадение запросу 
        bool isSynonimRequestOk(Item item, string request)
        {
            foreach (string s in item.listSynonims)
            {
                if (s.IndexOf(request.ToLower()) != -1 || s.ToLower().IndexOf(request) != -1)
                {
                    return true;
                }
            }
            return false;
        }

        void setItemVar1(Item it, string request, string levelLength, ContextMenuStrip cms)
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
            string s = name.ToLower();
            if (s.IndexOf(request.ToLower()) != -1 /*|| name.IndexOf(request) != -1 */|| isSynonimRequestOk(it, request.ToLower()))
                item.Enabled = true;
            else
                item.Enabled = false;
            cms.Items.Add(item);
        }

        bool hasChildren(ToolStripItem item, int level)
        {
            if (hasChildren(getThisItem(item, level)))
                return true;
            return false;
        }

        bool hasChildrenRequestOk(ToolStripItem item1, int level, string request)
        {
            Item item = getThisItem(item1, level);
            foreach (Item it in listItems)
            {
                string parent1 = "";
                string parent2 = "";
                string parent3 = "";

                if (it.level == 1)
                {
                    for (int i = 0; i < listItems.Count; i++)
                        //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                string s = listItems[i].name.ToLower();
                                if (listItems[i].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[i], request))
                                    return true;
                            }
                }
                else if (it.level == 2)
                {
                    for (int i = 0; i < listItems.Count; i++)
                        //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                parent1 = listItems[i].name;
                                for (int j = 0; j < listItems.Count; j++)
                                    //if (listItems[j].level == 2)
                                        if (listItems[j].parent == parent1)
                                        {
                                            string s = listItems[j].name.ToLower();
                                            if (listItems[j].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[j], request))
                                                return true;
                                        }
                            }

                }
                else if (it.level == 3)
                {
                    for (int i = 0; i < listItems.Count; i++)
                        //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                parent1 = listItems[i].name;
                                for (int j = 0; j < listItems.Count; j++)
                                    //if (listItems[j].level == 2)
                                        if (listItems[j].parent == parent1)
                                        {
                                            parent2 = listItems[j].name;
                                            for (int k = 0; k < listItems.Count; k++)
                                                //if (listItems[k].level == 3)
                                                    if (listItems[k].parent == parent2)
                                                    {
                                                        string s = listItems[k].name.ToLower();
                                                        if (listItems[k].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[k], request))
                                                            return true;
                                                    }
                                        }
                            }
                }
                else if (it.level == 4)
                {
                    for (int i = 0; i < listItems.Count; i++)
                        //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                parent1 = listItems[i].name;
                                for (int j = 0; j < listItems.Count; j++)
                                    //if (listItems[j].level == 2)
                                        if (listItems[j].parent == parent1)
                                        {
                                            parent2 = listItems[j].name;
                                            for (int k = 0; k < listItems.Count; k++)
                                                //if (listItems[k].level == 3)
                                                    if (listItems[k].parent == parent2)
                                                    {
                                                        parent3 = listItems[k].name;
                                                        for (int l = 0; l < listItems.Count; l++)
                                                            //if (listItems[l].level == 4)
                                                                if (listItems[l].parent == parent3)
                                                                {
                                                                    string s = listItems[l].name.ToLower();
                                                                    if (listItems[l].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[l], request))
                                                                        return true;
                                                                }
                                                    }
                                        }
                            }
                }
                
                
            }
            return false;
        }

        bool hasChildrenRequestOk(Item item, int level, string request)
        {
            try
            {
                foreach (Item it in listItems)
                {
                    string parent1 = "";
                    string parent2 = "";
                    string parent3 = "";

                    if (it.level == 1)
                    {
                        for (int i = 0; i < listItems.Count; i++)
                            //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                string s = listItems[i].name.ToLower();
                                if (listItems[i].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[i], request))
                                    return true;
                            }
                    }
                    else if (it.level == 2)
                    {
                        for (int i = 0; i < listItems.Count; i++)
                            //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                parent1 = listItems[i].name;
                                for (int j = 0; j < listItems.Count; j++)
                                    //if (listItems[j].level == 2)
                                    if (listItems[j].parent == parent1)
                                    {
                                        string s = listItems[j].name.ToLower();
                                        if (listItems[j].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[j], request))
                                            return true;
                                    }
                            }

                    }
                    else if (it.level == 3)
                    {
                        for (int i = 0; i < listItems.Count; i++)
                            //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                parent1 = listItems[i].name;
                                for (int j = 0; j < listItems.Count; j++)
                                    //if (listItems[j].level == 2)
                                    if (listItems[j].parent == parent1)
                                    {
                                        parent2 = listItems[j].name;
                                        for (int k = 0; k < listItems.Count; k++)
                                            //if (listItems[k].level == 3)
                                            if (listItems[k].parent == parent2)
                                            {
                                                string s = listItems[k].name.ToLower();
                                                if (listItems[k].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[k], request))
                                                    return true;
                                            }
                                    }
                            }
                    }
                    else if (it.level == 4)
                    {
                        for (int i = 0; i < listItems.Count; i++)
                            //if (listItems[i].level == 1)
                            if (listItems[i].parent == item.name)
                            {
                                parent1 = listItems[i].name;
                                for (int j = 0; j < listItems.Count; j++)
                                    //if (listItems[j].level == 2)
                                    if (listItems[j].parent == parent1)
                                    {
                                        parent2 = listItems[j].name;
                                        for (int k = 0; k < listItems.Count; k++)
                                            //if (listItems[k].level == 3)
                                            if (listItems[k].parent == parent2)
                                            {
                                                parent3 = listItems[k].name;
                                                for (int l = 0; l < listItems.Count; l++)
                                                    //if (listItems[l].level == 4)
                                                    if (listItems[l].parent == parent3)
                                                    {
                                                        string s = listItems[l].name.ToLower();
                                                        if (listItems[l].name.IndexOf(request) != -1 || s.IndexOf(request) != -1 || isSynonimRequestOk(listItems[l], request))
                                                            return true;
                                                    }
                                            }
                                    }
                            }
                    }


                }
                return false;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add(e.Message + "\n" + e.StackTrace);
                return false;
            }
        }

        Item getThisItem(ToolStripItem item, int level)
        {
            foreach (Item i in listItems)
            {
                string s = item.Text.ToLower();
                if (i.name.ToLower().IndexOf(s) != -1 && level == i.level)
                    return i;
            }
            return new Item();
        }

        

        //консоль + закрытый иерархический список
        //если нет запроса - обычное контекстное меню с поиском наверху
        void var2(string request/*запрос*/, ContextMenuStrip cms /*используемое контекстное меню*/)
        {
            try
            {
                //каждый список - подшедшие запросу пункты ветки одного из пунктов нулевого уровня
                List<List<Item>> searchBrunchResult = new List<List<Item>>();
                foreach (Item item in listItems)
                {
                    if (item.level == 0)
                    {
                        //получили ветку этого пункта
                        List<Item> brunch = getAllBranch(item);
                        //ветка с сорванными ненужными листьями
                        List<Item> newBrunch = new List<Item>();
                        //начинаем поиск по ветке
                        for (int i = 0; i < brunch.Count; i++)
                        {
                            //поиск по листьям
                            string s = brunch[i].name.ToLower();
                            if (s.IndexOf(request.ToLower()) != -1)
                            {
                                newBrunch.Add(brunch[i]);
                            }
                        }
                        searchBrunchResult.Add(newBrunch);
                    }
                }
                proc.AllEnabled(cms, false);
                //к этой строке у нас есть определенные пункты меню, удовлетворяющие запросу
                for (int i = 0; i < searchBrunchResult.Count; i++)
                {
                    for (int j = 0; j < searchBrunchResult[i].Count; j++)
                    {
                        string type = "";
                        switch (searchBrunchResult[i][j].level)
                        {
                            case 0:
                                {
                                    foreach (ToolStripItem item in cms.Items)
                                    {
                                        type = item.GetType().ToString();
                                        if (type != "System.Windows.Forms.ToolStripTextBox")
                                            if (item.Text == searchBrunchResult[i][j].name)
                                            {
                                                item.Enabled = true;
                                                proc.setAllParentsEnabled(item, searchBrunchResult[i][j].level);
                                                setChildrenEnabled(item);
                                            }
                                    }
                                }
                                break;
                            case 1:
                                {
                                    for (int k = 1; k < cms.Items.Count; k++)
                                    {
                                        type = cms.Items[k].GetType().ToString();
                                        if (type != "System.Windows.Forms.ToolStripSeparator")
                                            for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)
                                            {
                                                type = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                                                if (type != "System.Windows.Forms.ToolStripSeparator")
                                                    if (((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].Text == searchBrunchResult[i][j].name)
                                                    {
                                                        ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].Enabled = true;
                                                        proc.setAllParentsEnabled(((ToolStripMenuItem)cms.Items[k]).DropDownItems[l], searchBrunchResult[i][j].level);
                                                        setChildrenEnabled(((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]);
                                                    }
                                            }
                                    }
                                }
                                break;
                            case 2:
                                {
                                    for (int k = 1; k < cms.Items.Count; k++)
                                    {
                                        type = cms.Items[k].GetType().ToString();
                                        if (type != "System.Windows.Forms.ToolStripSeparator")
                                            for (int l = 0; l < ((ToolStripMenuItem)cms.Items[k]).DropDownItems.Count; l++)
                                            {
                                                type = ((ToolStripMenuItem)cms.Items[k]).DropDownItems[l].GetType().ToString();
                                                if (type != "System.Windows.Forms.ToolStripSeparator")
                                                    for (int m = 0; m < ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems.Count; m++)
                                                        if (((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].Text == searchBrunchResult[i][j].name)
                                                        {
                                                            ((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m].Enabled = true;
                                                            proc.setAllParentsEnabled(((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m], searchBrunchResult[i][j].level);
                                                            setChildrenEnabled(((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]);
                                                        }
                                            }
                                    }
                                }
                                break;
                            case 3:
                                {
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
                                                                if (((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n].Text == searchBrunchResult[i][j].name)
                                                                {
                                                                    ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n].Enabled = true;
                                                                    proc.setAllParentsEnabled(((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n], searchBrunchResult[i][j].level);
                                                                    setChildrenEnabled(((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n]);
                                                                }
                                                    }
                                            }
                                    }
                                }
                                break;
                            case 4:
                                {
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
                                                                        if (((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n]).DropDownItems[o].Text == searchBrunchResult[i][j].name)
                                                                        {
                                                                            ((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n]).DropDownItems[o].Enabled = true;
                                                                            //теперь нужно сделать Enabled = true каждому предку уже заэнабленных
                                                                            proc.setAllParentsEnabled(((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n]).DropDownItems[o], searchBrunchResult[i][j].level);
                                                                            setChildrenEnabled(((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)((ToolStripMenuItem)cms.Items[k]).DropDownItems[l]).DropDownItems[m]).DropDownItems[n]).DropDownItems[o]);
                                                                        }
                                                            }
                                                    }
                                            }
                                    }
                                }
                                break;
                        }
                    }
                }
                cms.Update();
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе var2./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе var2./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }
        }

        //делает всех детей доступными
        public void setChildrenEnabled(ToolStripItem item)
        {
            for (int i = 0; i < ((ToolStripMenuItem)item).DropDownItems.Count; i++)
            {
                ((ToolStripMenuItem)item).DropDownItems[i].Enabled = true;
                setChildrenEnabled(((ToolStripMenuItem)item).DropDownItems[i]);
            }
        }

        //консоль + часто используемые результаты
        //если нет запроса - часто используемые результаты
        void var3(string request/*запрос*/, string levelLength, Panel p /*используемое контекстное меню*/)
        {
            try
            {
                lsr.Clear();
                level = -1;
                findRequest(request.ToLower());
                //удаляем дублирующие
                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (lsr[i].name.TrimStart().IndexOf(lsr[j].name.TrimStart()) != -1 && lsr[i].parent == lsr[j].parent)
                            {
                                lsr.RemoveAt(j);
                                if (j - 1 > 0)
                                    j--;
                                else
                                    break;
                                if (i - 1 > 0)
                                    i--;
                            }
                        }
                    }
                }

                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (i < j && lsr[i].parent == lsr[j].name)
                            {
                                Item ite = lsr[i];
                                lsr[i] = lsr[j];
                                lsr[j] = ite;
                            }
                        }
                    }
                }
                clearMyPanel(panelSearch, 2);
                int maxKolWord = getMaxWord(lsr);
                int wdth = panelVar3.Width;
                foreach (Item sr in lsr)
                    if (wdth < maxWidthMyContextMenu)
                        wdth += setMaxWidth(maxKolWord, wdth);
                foreach (Item sr in lsr)
                    setItemVar3(sr, request, levelLength, panelSearch, wdth, maxKolWord);
                deltaInc3 = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе var3./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе var3./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }
        }

        int getMaxWord(List<Item> l)
        {
            int i = 0;
            foreach (Item item in l)
            {
                if (item.name.Length > i)
                    i = item.name.Length;
            }
            return i;
        }

        int deltaIncFastItem = 0;
        int panelFastItemsWidth = 0;
        int panelFastItemsHeight = 0;
        void getFastItemsForPanel()
        {
            try
            {
                //поиск обнулили
                panelTextBox.Text = "";
                //clearContextMenu(contextMenuRequest);
                List<Item> arrFastResult = new List<Item>();
                if (listItems.Count > fastResult)
                    for (int i = 0; i < fastResult; i++)
                    {
                        if (listItems[i].name != "-")
                            arrFastResult.Add(listItems[i]);
                        else
                        {
                            int j = i;
                            while (listItems[j].name == "-")
                            {
                                j++;
                                if (j < listItems.Count)
                                    arrFastResult.Add(listItems[j]);
                                else
                                    arrFastResult.Add(listItems[0]); ;
                            }
                        }
                    }
                else
                    for (int i = 0; i < listItems.Count; i++)
                    {
                        if (listItems[i].name != "-")
                            arrFastResult.Add(listItems[i]);
                        else
                        {
                            int j = i;
                            while (listItems[j].name == "-")
                            {
                                j++;
                                if (j < listItems.Count)
                                    arrFastResult.Add(listItems[j]);
                                else
                                    arrFastResult.Add(listItems[0]); ;
                            }
                        }
                    }
                //сортируем массив
                sort(arrFastResult);
                if (listItems.Count > fastResult)
                    for (int i = 0; i < fastResult; i++)
                    {
                        for (int j = fastResult; j < listItems.Count; j++)
                        {
                            if (listItems[j].name != "-")
                                if (arrFastResult[fastResult - 1].colClicks < listItems[j].colClicks)
                                {
                                    if (arrFastResult.IndexOf(listItems[j]) == -1)
                                    {
                                        arrFastResult[fastResult - 1] = listItems[j];
                                        sort(arrFastResult);
                                    }
                                }
                        }
                    }
                else
                    for (int i = 0; i < listItems.Count; i++)
                    {
                        for (int j = 0; j < listItems.Count; j++)
                        {
                            if (listItems[j].name != "-")
                                if (arrFastResult[listItems.Count - 1].colClicks < listItems[j].colClicks)
                                {
                                    if (arrFastResult.IndexOf(listItems[j]) == -1)
                                    {
                                        arrFastResult[listItems.Count - 1] = listItems[j];
                                        sort(arrFastResult);
                                    }
                                }
                        }
                    }
                deltaIncFastItem = 0;
                clearMyPanel(panelSearch, 2);
                int badResult = 0;
                if (listItems.Count > fastResult)
                    for (int i = 0; i < fastResult; i++)
                    {
                        if (arrFastResult[i].colClicks >= kolCliks)
                        {
                            int x = firstItemPanelSearch.Location.X;
                            int y = firstItemPanelSearch.Location.Y;
                            int width = panelVar3.Size.Width;
                            int otstup = myOtstup;
                            int height = panelVar3.Size.Height;
                            if (!hasChildren(arrFastResult[i]))
                            {
                                addMyFastItemVar3(panelSearch, arrFastResult[i].name, parentLine(arrFastResult[i]), arrFastResult[i].parent, x, deltaIncFastItem + otstup + y, width, height, arrFastResult[i].level);
                                deltaIncFastItem += height;
                                panelFastItemsWidth = defaultPanelSearchSize.Width;
                                panelFastItemsHeight = deltaIncFastItem + topLeftFirstItemPanelSearch.Y + 2 * myOtstup;
                            }
                        }
                        else
                        { badResult++; }
                        if (badResult == fastResult)
                            panelFastItemsHeight = defaultPanelSearchSize.Height;
                    }
                else
                {
                    for (int i = 0; i < arrFastResult.Count; i++)
                    {
                        if (arrFastResult[i].colClicks >= kolCliks)
                        {
                            int x = firstItemPanelSearch.Location.X;
                            int y = firstItemPanelSearch.Location.Y;
                            int width = panelVar3.Size.Width;
                            int otstup = myOtstup;
                            int height = panelVar3.Size.Height;
                            if (!hasChildren(arrFastResult[i]))
                            {
                                addMyFastItemVar3(panelSearch, arrFastResult[i].name, parentLine(arrFastResult[i]), arrFastResult[i].parent, x, deltaIncFastItem + otstup + y, width, height, arrFastResult[i].level);
                                deltaIncFastItem += height;
                                panelFastItemsWidth = defaultPanelSearchSize.Width;
                                panelFastItemsHeight = deltaIncFastItem + topLeftFirstItemPanelSearch.Y + 2 * myOtstup;
                            }
                        }
                    }
                }
                if (arrFastResult.Count == 0)
                    panelFastItemsHeight = defaultPanelSearchSize.Height;
                if (panelFastItemsWidth == 0)
                    panelFastItemsWidth = defaultPanelSearchSize.Width;
                if (panelFastItemsHeight == 0)
                    panelFastItemsHeight = defaultPanelSearchSize.Height;
                
                setPanelSize(panelSearch, panelFastItemsWidth, panelFastItemsHeight);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе getFastItemsForPanel./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе getFastItemsForPanel./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }
        }

        //для часто используемых (с возможностью обнуления)
        void addMyFastItemVar3(Panel panel, string text, string pLine, string parent, int x, int y, int width, int height, int level)
        {
            Panel p = new Panel();
            Label label = new Label();
            PictureBox pb = new PictureBox();
            Label labelParent = new Label();
            Label labelLevel = new Label();
            Label parentLine = new Label();
            parentLine.Name = "parentLine";
            parentLine.Visible = false;
            parentLine.Text = pLine;
            labelLevel.Name = "level";
            labelLevel.Visible = false;
            labelLevel.Text = level.ToString();
            label.Font = p.Font = font;
            label.Text = text;
            label.Name = "name";
            label.Visible = true;
            labelParent.Visible = false;
            labelParent.Text = parent;
            labelParent.Name = "parent";
            p.Location = new Point(x, y);
            p.Size = new Size(width, height);
            p.Controls.Add(label);
            label.Location = new Point(3, 5);
            pb.Size = new Size(16, 16);
            pb.Location = new Point(126, 5);
            pb.Image = Image.FromFile(filenameDeleteFar);
            pb.Visible = true;
            p.MouseDown += new MouseEventHandler(pnl_MouseDown);
            p.MouseHover += new EventHandler(panel_MouseHover);
            p.MouseLeave += new EventHandler(panel_MouseLeave);
            p.BackColorChanged += new EventHandler(pnl_BackColorChanged);
            label.MouseHover += new EventHandler(lbl_MouseHover);
            label.MouseLeave += new EventHandler(lbl_MouseLeave);
            label.MouseDown += new MouseEventHandler(lbl_MouseDown);
            label.MouseUp += new MouseEventHandler(lbl_MouseUp);
            label.Click += new EventHandler(labelItem_Click);
            pb.Click += new EventHandler(pbDelete_Click);
            pb.MouseHover += new EventHandler(pb_MouseHover);
            pb.MouseLeave += new EventHandler(pb_MouseLeave);
            panel.Controls.Add(p);
            p.Controls.Add(pb);
            p.Controls.Add(label);
            p.Controls.Add(labelParent);
            p.Controls.Add(labelLevel);
            p.Controls.Add(parentLine);
        }

        //для результатов поиска
        int deltaInc3 = 0;
        void setItemVar3(Item item, string request, string levelLength, Panel p, int maxwidth, int maxkolwords)
        {
            string space = "";
            string name = item.name;
            switch (item.level)
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
            name = space + name;
            string s = name.ToLower();
            int x = topLeftFirstItemPanelSearch.X;
            int y = topLeftFirstItemPanelSearch.Y;
            int delta = panelVar3.Size.Height;
            int width = panelVar3.Size.Width;
            int otstup = myOtstup;
            int height = panelVar3.Size.Height;
            addMyItemRichTextBox(p, name, item, request.ToLower(), x, deltaInc3 + otstup + y, width, height, maxwidth, maxkolwords);
            deltaInc3 += delta;
        }

        void findRequest(string request)
        {
            try
            {
                level++;
                foreach (Item item in listItems)
                {
                    if (level == item.level)
                    {
                        string sItem = item.name.ToLower();
                        if (sItem.IndexOf(request) != -1 && level == item.level /*|| item.name.IndexOf(request) != -1 && level == item.level*/)
                        {
                            //отец добавляемого пункта
                            Item it = getParent(item);
                            if (it != null && !isIn(lsr, it))
                                lsr.Add(it);
                            if (!isIn(lsr, item))
                                    lsr.Add(item);
                        }
                        if (hasChildren(item))
                        {
                            if (hasChildrenRequestOk(item, level, request))
                            {
                                if (!isIn(lsr, item))
                                    lsr.Add(item);
                            }
                            findRequest(request);
                        }
                        if (sItem.IndexOf(request) == -1 /*&& item.name.IndexOf(request) == -1*/)
                        {
                            if (isSynonimRequestOk(item, request))
                                if (!isIn(lsr, item))
                                    lsr.Add(item);
                        }
                    }
                }
                level--;
            }
            catch (Exception e)
            {
                logErorrs.Add(e.Message +"\n" + e.StackTrace);
                MessageBox.Show(e.Message + "\n" + e.StackTrace, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        Item getParent(Item i)
        {
            foreach (Item item in listItems)
            {
                if (i.parent == item.name && i.level == item.level + 1)
                {
                    return item;
                }
            }
            return null;
        }

        //консоль + избранные результаты
        //если нет запроса - избранные результаты
        void var4(string request/*запрос*/, string levelLength, Panel panel /*используемое контекстное меню*/)
        {
            try
            {
                favoritesShow = false;
                lsr.Clear();
                level = -1;
                findRequest(request.ToLower());
                //удаляем дублирующие
                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (lsr[i].name.TrimStart().IndexOf(lsr[j].name.TrimStart()) != -1 && lsr[i].parent == lsr[j].parent)
                            {
                                lsr.RemoveAt(j);
                                if (j - 1 > 0)
                                    j--;
                                else
                                    break;
                                if (i - 1 > 0)
                                    i--;
                            }
                        }
                    }
                }

                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (i < j && lsr[i].parent == lsr[j].name)
                            {
                                Item ite = lsr[i];
                                lsr[i] = lsr[j];
                                lsr[j] = ite;
                            }
                        }
                    }
                }
                clearMyPanel(panelSearch, 2);
                int maxKolWord = getMaxWord(lsr);
                int wdth = panelVar3.Width;
                foreach (Item sr in lsr)
                    if (wdth < maxWidthMyContextMenu)
                        wdth += setMaxWidth(maxKolWord, wdth);
                foreach (Item sr in lsr)
                    setItemVar4(sr, request, levelLength, panelSearch, wdth, maxKolWord);
                deltaInc4 = 0;
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе var4./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе var4./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }
        }
        
        void addMyItemFavoritesVar4(Panel panel, string text, string pLine, string parent, bool favorit, int x, int y, int width, int height, int level)
        {
            Panel p = new Panel();
            Label label = new Label();
            PictureBox pb = new PictureBox();
            Label labelParent = new Label();
            Label labelLevel = new Label();
            Label labelFavorit = new Label();
            Label parentLine = new Label();
            parentLine.Name = "parentLine";
            parentLine.Visible = false;
            parentLine.Text = pLine;
            label.Name = "name";
            labelParent.Name = "parent";
            labelLevel.Name = "level";
            labelFavorit.Name = "favorit";
            if (favorit)
                labelFavorit.Text = "favorit";
            else
                labelFavorit.Text = "not favorit";
            labelFavorit.Visible = false;
            label.Font = p.Font = font;
            label.Text = text;
            label.Visible = true;
            labelParent.Visible = false;
            labelParent.Text = parent;
            labelLevel.Visible = false;
            labelLevel.Text = level.ToString();
            p.Location = new Point(x, y);
            p.Size = new Size(width, height);
            label.Location = new Point(3, 5);
            pb.Size = new Size(16, 16);
            pb.Location = new Point(126, 5);
            pb.Image = Image.FromFile(filenameFavorit);
            pb.Visible = true;
            p.MouseDown += new MouseEventHandler(pnl_MouseDown);
            p.MouseHover += new EventHandler(panel_MouseHover);
            p.MouseLeave += new EventHandler(panel_MouseLeave);
            p.BackColorChanged += new EventHandler(pnl_BackColorChanged);
            label.MouseHover += new EventHandler(lbl_MouseHover);
            label.MouseLeave += new EventHandler(lbl_MouseLeave);
            label.MouseDown += new MouseEventHandler(lbl_MouseDown);
            label.MouseUp += new MouseEventHandler(lbl_MouseUp);
            label.Click += new EventHandler(labelItem_Click);
            pb.Click += new EventHandler(pbDelete_Click);
            pb.MouseHover += new EventHandler(pb_MouseHover);
            pb.MouseLeave += new EventHandler(pb_MouseLeave);
            panel.Controls.Add(p);
            p.Controls.Add(pb);
            p.Controls.Add(label);
            p.Controls.Add(labelParent);
            p.Controls.Add(labelLevel);
            p.Controls.Add(labelFavorit);
            p.Controls.Add(parentLine);
        }

        //для результатов поиска
        int deltaInc4 = 0;
        void setItemVar4(Item item, string request, string levelLength, Panel p, int maxkolword, int maxkolwords)
        {
            string space = "";
            string name = item.name;
            switch (item.level)
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
            name = space + name;
            string s = name.ToLower();
            int x = topLeftFirstItemPanelSearch.X;
            int y = topLeftFirstItemPanelSearch.Y;
            int delta = panelVar3.Size.Height;
            int width = panelVar3.Size.Width;
            int otstup = myOtstup;
            int height = panelVar3.Size.Height;
            addMyItemRichTextBox(p, name, item, request.ToLower(), x, deltaInc4 + otstup + y, width, height, maxkolword, maxkolwords);
            deltaInc4 += delta;
        }



        //консоль + часто используемые результаты + иерархический список, \nзаменяющийся на развернутое дерево при поиске.
        //если нет запроса - обычное контекстное меню с поиском наверху + часто используемые пункты
        void var5(string request/*запрос*/, string levelLength, Panel p)
        {
            try
            {
                clearMyPanel(p, 0);
                lsr.Clear();
                level = -1;
                findRequest(request.ToLower());
                //удаляем дублирующие
                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (lsr[i].name.TrimStart().IndexOf(lsr[j].name.TrimStart()) != -1 && lsr[i].parent == lsr[j].parent)
                            {
                                lsr.RemoveAt(j);
                                if (j - 1 > 0)
                                    j--;
                                else
                                    break;
                                if (i - 1 > 0)
                                    i--;
                            }
                        }
                    }
                }

                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (i < j && lsr[i].parent == lsr[j].name)
                            {
                                Item ite = lsr[i];
                                lsr[i] = lsr[j];
                                lsr[j] = ite;
                            }
                        }
                    }
                }
                clearMyPanel(p, 0);
                panelWidth = 0;
                panelHeight = 0;
                int maxKolWord = getMaxWord(lsr);
                int wdth = panelVar3.Width;
                foreach (Item sr in lsr)
                    if (wdth < maxWidthMyContextMenu)
                        wdth += setMaxWidth(maxKolWord, wdth);
                foreach (Item item in lsr)
                    setItemVar56(item, request, levelLength, p, wdth, maxKolWord);
                deltaInc56 = 0;
                
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе var5./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе var5./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }
        }

        

        int deltaInc56 = 0;
        void setItemVar56(Item it, string request, string levelLength, Panel p, int maxwidth, int maxkolwords)
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
            name = space + name;
            string s = name.ToLower();
            int x = 0;
            int y = 0;
            int delta = panelVar3.Size.Height;
            int width = panelVar3.Size.Width;
            int otstup = myOtstup;
            int height = panelVar3.Size.Height;
            addMyItemRichTextBox(p, name, it, request.ToLower(), x, deltaInc56 + otstup, width, height, maxwidth, maxkolwords);
            deltaInc56 += delta;
        }

        void clearMyPanel(Panel p, int idx /*с какого индекса в списке чистить*/)
        {
            for (int i = idx; i < p.Controls.Count; i++)
            {
                string type = p.Controls[i].GetType().ToString();
                if (type != "System.Windows.Forms.TextBox")
                {
                    p.Controls.RemoveAt(i);
                    i--;
                }
            }
        }

        //консоль + избранные пункты + иерархический список, \nзаменяющийся на развернутое дерево при поиске.
        //если нет запроса - обычное контекстное меню с поиском наверху + избранные пункты
        void var6(string request/*запрос*/, string levelLength, Panel p)
        {
            try
            {
                clearMyPanel(p, 0);
                favoritesShow = false;
                lsr.Clear();
                level = -1;
                findRequest(request.ToLower());
                //удаляем дублирующие
                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (lsr[i].name.TrimStart().IndexOf(lsr[j].name.TrimStart()) != -1 && lsr[i].parent == lsr[j].parent)
                            {
                                lsr.RemoveAt(j);
                                if (j - 1 > 0)
                                    j--;
                                else
                                    break;
                                if (i - 1 > 0)
                                    i--;
                            }
                        }
                    }
                }

                for (int i = 0; i < lsr.Count; i++)
                {
                    for (int j = 0; j < lsr.Count; j++)
                    {
                        //не сравнивать самого с собой
                        if (i != j)
                        {
                            if (i < j && lsr[i].parent == lsr[j].name)
                            {
                                Item ite = lsr[i];
                                lsr[i] = lsr[j];
                                lsr[j] = ite;
                            }
                        }
                    }
                }
                clearMyPanel(p, 0);
                panelWidth = 0;
                panelHeight = 0;
                int maxKolWord = getMaxWord(lsr);
                int wdth = panelVar3.Width;
                foreach (Item sr in lsr)
                    if (wdth < maxWidthMyContextMenu)
                        wdth += setMaxWidth(maxKolWord, wdth);
                foreach (Item item in lsr)
                    setItemVar56(item, request, levelLength, p, wdth, maxKolWord);
                deltaInc56 = 0;
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе var6./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе var6./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }
        }

        //заполняет стек (список) всеми пунктами, подчиненными этому (включая этот)
        List<Item> getAllBranch(Item item)
        {
            List<Item> branch = new List<Item>();
            branch.Add(item);
            Item oldItem = new Item();
            oldItem = item;
            for (int i = 0; i < listItems.Count; i++)
            {
                if (item.name == listItems[i].parent)
                {
                    Item itemm = listItems[i];
                    branch.Add(listItems[i]);
                    oldItem = listItems[i];
                    
                    while (oldItem.name == listItems[i].parent)
                    {
                        if (i + 1 < listItems.Count)
                            i++;
                        itemm = listItems[i];
                        branch.Add(listItems[i]);
                        oldItem = listItems[i];
                        //if (i + 1 < listItems.Count)
                        //    i++;
                    }

                }
                else
                {
                    for (int j = 0; j < branch.Count; j++)
                    {
                        if (listItems[i].parent == branch[j].name)
                        {
                            branch.Add(listItems[i]);
                            oldItem = listItems[i];
                        }
                    }
                }
            }
            return branch;
        }

        //сортирует заданный массив по убыванию
        void sort(List<Item> arrFastResult)
        {
            if (listItems.Count > fastResult)
                for (int i = 0; i < fastResult; i++)
                {
                    for (int j = 0; j < fastResult; j++)
                    {
                        Item t = new Item();
                        int a = arrFastResult[i].colClicks;
                        int b = arrFastResult[j].colClicks;
                        if (a > b)
                        {
                            t = arrFastResult[j];
                            arrFastResult[j] = arrFastResult[i];
                            arrFastResult[i] = t;
                        }
                    }
                }
            else
                for (int i = 0; i < listItems.Count; i++)
                {
                    for (int j = 0; j < listItems.Count; j++)
                    {
                        Item t = new Item();
                        int a = arrFastResult[i].colClicks;
                        int b = arrFastResult[j].colClicks;
                        if (a > b)
                        {
                            t = arrFastResult[j];
                            arrFastResult[j] = arrFastResult[i];
                            arrFastResult[i] = t;
                        }
                    }
                }

        }

        List<Item> getAllFavorites()
        {
            List<Item> l = new List<Item>();
            foreach (Item item in listItems)
            {
                if (item.favorit)
                    l.Add(item);
            }
            return l;
        }

        List<string> getAllKolClicks()
        {
            List<string> l = new List<string>();
            foreach (Item item in listItems)
            {
                l.Add(item.name + "(level:" + item.level + "; parent:" + item.parent + ") = " + item.colClicks );
            }
            return l;
        }

        int deltaIncFavoritItem = 0;
        int panelFavoriteWidth = 0;
        int panelFavoriteHeight = 0;
        void getFavoritesForPanel()
        {
            try
            {
                favoritesShow = true;
                //поиск обнулили
                panelTextBox.Text = "";
                //List<Item> allFavorites = new List<Item>();
                if (newOpen)
                {
                    foreach (Item item in listItems)
                    {
                        if (item.favorit)
                            allFavorites.Add(item);
                    }
                    newOpen = false;
                }
                deltaIncFavoritItem = 0;
                clearMyPanel(panelSearch, 2);
                for (int i = 0; i < allFavorites.Count; i++)
                {
                    int x = firstItemPanelSearch.Location.X;
                    int y = firstItemPanelSearch.Location.Y;
                    int width = panelVar3.Size.Width;
                    int otstup = myOtstup;
                    int height = panelVar3.Size.Height;
                    if (!hasChildren(allFavorites[i]))
                    {
                        addMyItemFavoritesVar4(panelSearch, allFavorites[i].name, parentLine(allFavorites[i]), allFavorites[i].parent, allFavorites[i].favorit, x, deltaIncFavoritItem + otstup + y, width, height, allFavorites[i].level);
                        deltaIncFavoritItem += height;
                        panelFavoriteWidth = defaultPanelSearchSize.Width;
                        panelFavoriteHeight = deltaIncFavoritItem + topLeftFirstItemPanelSearch.Y + 2 * myOtstup;
                    }
                }
                if (allFavorites.Count == 0)
                    panelFavoriteHeight = defaultPanelSearchSize.Height;
                if (panelFavoriteWidth == 0)
                    panelFavoriteWidth = defaultPanelSearchSize.Width;
                if (panelFavoriteHeight == 0)
                    panelFavoriteHeight = defaultPanelSearchSize.Height;

                setPanelSize(panelSearch, panelFavoriteWidth, panelFavoriteHeight);
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе getFastItemsForPanel./n" + e.StackTrace.ToString() + "/n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе getFastItemsForPanel./n" + e.StackTrace.ToString() + "/n" + e.Message);
            }

            
        }

        void resizePanelSearch()
        { 
            int i = 0;
            foreach (Object obj in panelSearch.Controls)
                if (obj is Label)
                {
                    string s = ((Label)obj).Text.Trim().ToLower();
                    if (((Label)obj).Visible && s != "x")
                    {
                        i++;
                    }
                }
            setPanelSize(panelSearch,defaultPanelSearchSize.Width, 3 + panelTextBox.Size.Height + 3 + i*15 + 5);
        }

        void setFavoritesForPanel(string name, bool ok)
        {
            foreach (Item i in listItems)
                if (name.TrimStart().IndexOf(i.name) != -1)
                    i.favorit = ok;
        }

        string parentLine(Item i)
        {
            List<Item> l = getAllParents(i);
            if (l.Count != 0)
            {
                string s = "";
                int k = 0;
                foreach (Item it in l)
                {
                    if (k != 0)
                        s += "\\";
                    s += it.name;
                    k++;
                }
                return s + "";
            }
            return "";
        }

        void setItemNameOnLabel(string name, Label label, Panel p)
        {
            if (name.ToLower() == "x")
            { 
                label.Font = new Font("Microsoft Sans Serif", (float)9, FontStyle.Regular);
                label.ForeColor = Color.Red;
            }
            label.Text = name;
            p.Size = new Size(defaultPanelSearchSize.Width, p.Size.Height + labelDeleteItem1.Size.Height);
        }

        //добавляет строку в лог
        void setLog(string s)
        {
            logOther.Add(s);
        }

        private void contextMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text != "" && e.ClickedItem.Text != "-" && ((ToolStripMenuItem)e.ClickedItem).DropDownItems.Count == 0)
            {
                setLog("Пункт меню \"" + e.ClickedItem.Text.TrimStart() + "\" выполнено.");
                richTextBoxLog.Text = sepLog + "\"" + e.ClickedItem.Text.TrimStart() + "\" выполнено.\n" + richTextBoxLog.Text;
            }
        }

        private void listBoxDynamic_MouseMove(object sender, MouseEventArgs e)
        {
            mouseX = e.X;
            mouseY = e.Y;
            //coord.Text = "Ox:" + mouseX.ToString() + "; Oy:" + mouseY + ".";
        }

        private void toolStripStatusLabel2_Click(object sender, EventArgs e)
        {
            currentVariant.Text = sender.ToString();
        }

        private void contextMenu_Opened(object sender, EventArgs e)
        {
            try
            {
                idxFocusetItemDown = 0;
                if (wasDownload)
                {
                    if (textBoxSearch.Visible)
                    {
                        textBoxSearch.TextBox.ContextMenu = null;
                        textBoxSearch.TextBox.ContextMenuStrip = null;
                        textBoxSearch.Focus();
                        
                    }
                }
                else
                {
                    MessageBox.Show("Вы не загрузили контекстное меню из файла.\nЧтобы загрузить его нажмите на пункт меню \"Файл\"->\"Открыть\" или воспользуйтесь клавишами Alt+O.", "Контекстное меню не было загружено!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    contextMenu.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в методе contextMenu_Opened\n" + ex.StackTrace + "\n" + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе contextMenu_Opened\n" + ex.StackTrace + "\n" + ex.Message);
            }
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            try
            {
                clearContextMenu(contextMenu);
                setContextMenu(contextMenu);
                string request = "";
                if (currentVariant.Text == "Вариант 1" || currentVariant.Text == "Вариант 2")
                    request = textBoxSearch.Text;
                else
                {
                
                    request = panelTextBox.Text;
                }
                if (request != "")
                {
                    if (currentVariant.Text == "Вариант 3" || currentVariant.Text == "Вариант 4")
                    {
                        clearMyPanel(panelSearch, 2);
                    }
                    lsr.Clear();
                    search(request);
                    if (lsr.Count == 0)
                        nothingFound = true;
                    if (nothingFound)
                    {
                        Label label = new Label();
                        label.Font = new Font(myFont, (float)myFontSize, FontStyle.Italic);
                        label.Name = "LabelNothingFound";
                        label.Text = "Результатов не найдено";
                        if (label.Width < panelSearch.Width)
                            label.Size = new Size(panelSearch.Width, 15);
                        else
                            label.Size = new Size(label.Width, 15);
                        
                        if (currentVariant.Text == "Вариант 3" || currentVariant.Text == "Вариант 4")
                        {
                            label.Location = new Point(topLeftFirstItemPanelSearch.X, topLeftFirstItemPanelSearch.Y);
                            clearMyPanel(panelSearch, 2);
                            panelSearch.Controls.Add(label);
                            panelSearch.Size = new Size(panelSearch.Width, 50);
                        }
                        else
                        {
                            label.Location = new Point(3, 5);
                            clearMyPanel(myContextMenu, 0);
                            myContextMenu.Controls.Add(label);
                            myContextMenu.Size = new Size(myContextMenu.Width, 25);
                        }
                        nothingFound = false;
                    }
                }
                else
                {
                    if (currentVariant.Text == "Вариант 1" || currentVariant.Text == "Вариант 2")
                    {
                        clearContextMenu(contextMenu);
                        setContextMenu(contextMenu);
                    }
                    else if (currentVariant.Text == "Вариант 3")
                    {
                        getFastItemsForPanel();
                    }
                    else if (currentVariant.Text == "Вариант 4")
                    {
                        getFavoritesForPanel();
                    }
                    else if (currentVariant.Text == "Вариант 5")
                    {
                        clearMyPanel(myContextMenu, 0);
                        setMyContextMenu();
                        myContextMenu.Size = new Size(defaultWidthCMS, defaultHeightCMS);
                        panelSearch.Size = new Size(defaultWidthSearch, defaultHeightSearch);
                        getFastItemsForPanel();
                    }
                    else if (currentVariant.Text == "Вариант 6")
                    {
                        clearMyPanel(myContextMenu, 0);
                        setMyContextMenu();
                        myContextMenu.Size = new Size(defaultWidthCMS, defaultHeightCMS);
                        panelSearch.Size = new Size(defaultWidthSearch, defaultHeightSearch);
                        getFavoritesForPanel();
                    }

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в методе textBoxSearch_TextChanged./n" + ex.StackTrace.ToString() + "/n" + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе textBoxSearch_TextChanged./n" + ex.StackTrace.ToString() + "/n" + ex.Message);
            }
        }
        
        private void contextMenu_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                mouseRightButtonClick = true;
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            //записываем все данные о пунктах контекстного меню по завершении работы
            //saveAllItems();
            saveAllTools();
            saveToTxt(Application.StartupPath + "\\#log.txt");
        }

        private void clearLogDownloadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logOther.Clear();    
        }

        private void clearLogErrorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logErorrs.Clear();
        }

        private void заТекущийСеансToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Log log = new Log();
                createTxtFile(Application.StartupPath + "\\#temp.txt");
                log.shortFilename = "#temp.txt";
                log.Show();
                FileInfo fi = new FileInfo(Application.StartupPath + "\\#temp.txt");
                fi.Delete();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка в методе очиститьСведенияToolStripMenuItem_Click./n" + ex.StackTrace.ToString() + "/n" + ex.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе очиститьСведенияToolStripMenuItem_Click./n" + ex.StackTrace.ToString() + "/n" + ex.Message);
            }
        }

        private void заВсёВремяToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log log = new Log();
            saveToTxt(Application.StartupPath + "\\#log.txt");
            log.shortFilename = "#log.txt";
            log.Show();
        }

        private void panelSearch_VisibleChanged(object sender, EventArgs e)
        {
            if (panelSearch.Visible)
            {
                if (currentVariant.Text == "Вариант 3")
                {
                    getFastItemsForPanel();
                }
                else if (currentVariant.Text == "Вариант 4")
                {
                    if (wasDownload)
                    {
                        getFavoritesForPanel();
                    }
                }
                else if (currentVariant.Text == "Вариант 5")
                {
                    if (wasDownload)
                    {

                        getFastItemsForPanel();
                    }

                }
                else if (currentVariant.Text == "Вариант 6")
                {
                    if (wasDownload)
                    {
                        panelSearch.Size = new Size(defaultPanelSearchSize.Width, defaultPanelSearchSize.Height);
                        getFavoritesForPanel();
                    }
                }
                panelSearch.Focus();
                panelTextBox.Focus();
            }
            else
            {
                labelToolTip.Visible = false;
            }
        }
        private void listBoxDynamic_MouseDown(object sender, MouseEventArgs e)
        {
            if (currentVariant.Text == "Вариант 5" || currentVariant.Text == "Вариант 6")
            {
                mouseX = e.X;
                mouseY = e.Y;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (wasDownload)
                    {
                        myContextMenu.Location = new Point(mouseX + listBoxDynamic.Location.X, mouseY + listBoxDynamic.Location.Y);
                        //панель отсчитывает координаты от groopBoxDynamic, поэтому необходима поправка на локацию listBoxDynamic
                        panelSearch.Location = new Point(mouseX + myContextMenu.Width + listBoxDynamic.Location.X, mouseY + listBoxDynamic.Location.Y);
                        panelSearch.Visible = true;
                        myContextMenu.Visible = true;
                        curentPanel = myContextMenu;
                    }
                    else
                        MessageBox.Show("Вы не загрузили контекстное меню из файла.\nЧтобы загрузить его нажмите на пункт меню \"Файл\"->\"Открыть\" или воспользуйтесь клавишами Alt+O.", "Контекстное меню не было загружено!",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    myContextMenu.Visible = false;
                    panelSearch.Visible = false;
                }
            }
            if (currentVariant.Text == "Вариант 3" || currentVariant.Text == "Вариант 4")
            {
                mouseX = e.X;
                mouseY = e.Y;
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    if (wasDownload)
                    {
                        panelSearch.Location = new Point(mouseX + listBoxDynamic.Location.X, mouseY + listBoxDynamic.Location.Y);
                        panelSearch.Visible = true;
                        myContextMenu.Visible = false;
                        curentPanel = panelSearch;
                    }
                    else
                        MessageBox.Show("Вы не загрузили контекстное меню из файла.\nЧтобы загрузить его нажмите на пункт меню \"Файл\"->\"Открыть\" или воспользуйтесь клавишами Alt+O.", "Контекстное меню не было загружено!",MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (e.Button == System.Windows.Forms.MouseButtons.Left)
                {
                    panelSearch.Visible = false;
                }
            }
        }

        //чтобы родителей не печатал
        string getS(string s)
        {
            if (s.IndexOf("|") != -1)
                return s.Substring(s.IndexOf("|"), s.Length);
            else
                return "";
        }

        void setPanelSize(Panel p, int widht, int height)
        {
            p.Size = new Size(widht, height);
        }

        private void contextMenu_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            textBoxSearch.Clear();
            clearContextMenu(contextMenu);
            setContextMenu(contextMenu);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(new System.Globalization.CultureInfo("ru-RU"));
            setAllTools();
            MyOpen();
            this.WindowState = FormWindowState.Maximized;
            textBoxFileName.Text = "Сейчас используется: " + buttonVar1.Text + " ; " + myShortFileName + ".";

        }

        void setMyContextMenu()
        {
            //верхний левый угол самого верхнего пункта меню
            int topLocationX = pnl.Location.X;
            int topLocationY = pnl.Location.Y;
            List<List<Item>> list = new List<List<Item>>();
            //списки для каждого уровня иерархии
            List<Item> l1 = new List<Item>();
            List<Item> l2 = new List<Item>();
            List<Item> l3 = new List<Item>();
            List<Item> l4 = new List<Item>();
            List<Item> l5 = new List<Item>();
            for (int idx = 0; idx < listItems.Count; idx++)
            {

                switch (listItems[idx].level)
                {
                    case 0: l1.Add(listItems[idx]);
                        break;
                    case 1: l2.Add(listItems[idx]);
                        break;
                    case 2: l3.Add(listItems[idx]);
                        break;
                    case 3: l4.Add(listItems[idx]);
                        break;
                    case 4: l5.Add(listItems[idx]);
                        break;
                    default: l1.Add(listItems[idx]);
                        break;
                }
            }
            list.Add(l1);
            list.Add(l2);
            list.Add(l3);
            list.Add(l4);
            list.Add(l5);
            int x = topLocationX;
            int y = topLocationY;
            int delta = pnl.Height;
            int width = pnl.Width;
            int deltainc = 0;
            int otstup = 5;
            int height = pnl.Height;
            bool wasSeparator = false;
            bool begin = true;
            for (int i = 0; i < listItems.Count; i++)
            {
                if (listItems[i].level == 0)
                {
                    if (listItems[i].name != "-")
                    {
                        if (begin)
                        {
                            if (hasChildren(listItems[i]))
                                addMyItem(myContextMenu, listItems[i], listItems[i].name, listItems[i].parent, listItems[i].favorit, true, x, 3, width, height, listItems[i].level);
                            else
                                addMyItem(myContextMenu, listItems[i], listItems[i].name, listItems[i].parent, listItems[i].favorit, false, x, 3, width, height, listItems[i].level);
                            begin = false;
                        }
                        else
                            if (hasChildren(listItems[i]))
                                addMyItem(myContextMenu, listItems[i], listItems[i].name, listItems[i].parent, listItems[i].favorit, true, x, deltainc - 2 + otstup, width, height, listItems[i].level);
                            else
                                addMyItem(myContextMenu, listItems[i], listItems[i].name, listItems[i].parent, listItems[i].favorit, false, x, deltainc - 2 + otstup, width, height, listItems[i].level);
                        
                        deltainc += delta;
                        maxHeightCMS += delta + otstup;
                    }
                    else
                    {
                        if (hasChildren(listItems[i]))
                            addMyItem(myContextMenu, listItems[i], listItems[i].name, listItems[i].parent, listItems[i].favorit, true, x, deltainc + otstup, width, height, listItems[i].level);
                        else
                            addMyItem(myContextMenu, listItems[i], listItems[i].name, listItems[i].parent, listItems[i].favorit, false, x, deltainc + otstup, width, height, listItems[i].level);
                        maxHeightCMS += delta / 2;
                        if (!wasSeparator)
                            wasSeparator = true;
                        else
                            wasSeparator = false;
                        deltainc += delta / 2;
                    }
                    myContextMenu.Size = new Size(maxWidthCMS, maxHeightCMS);
                    
                }
            }
            
        }

        bool wasSeparator = false;
        void addMyItem(Panel panel, Item item, string text, string parent, bool favorit, bool dropDownItems, int x, int y,/*сдвиг вниз относительно верхнего соседа*/ int width, int height, int level)
        {
            if (text == "-")
                wasSeparator = true;
            else
                wasSeparator = false;
            if (wasSeparator)
            {
                Panel p = new Panel();
                wasSeparator = false;
                p.Location = new Point(x, y + 5);
                p.Size = new Size(width, 1);
                p.BackColor = Color.LightGray;
                panel.Controls.Add(p);
            }
            else
            {
                panel.TabIndex++;
                Label label = new Label();
                Label labelParent = new Label();
                Label labelLevel = new Label();
                Label labelFavorit = new Label();
                Label dropDown = new Label();
                Panel p = new Panel();
                if (currentVariant.Text == "Вариант 6")
                {
                    if (!hasChildren(item))
                    {
                        PictureBox pb = new PictureBox();
                        pb.Size = new Size(16, 16);

                        if (!favorit)
                        {
                            pb.Image = Image.FromFile(filenameFavoritFar);
                            labelFavorit.Text = "not favorit";
                        }
                        else
                        {
                            pb.Image = Image.FromFile(filenameFavorit);
                            labelFavorit.Text = "favorit";
                        }
                        pb.Location = new Point(160, 4);
                        p.Controls.Add(pb);
                        pb.MouseHover += new EventHandler(pbMenuItem_MouseHover);
                        pb.MouseLeave += new EventHandler(pbMenuItem_MouseLeave);
                        pb.Click += new EventHandler(pbMenuItem_Click);
                    }
                }
                label.Font = dropDown.Font = p.Font = font;
                labelParent.Text = parent;
                label.Name = "name";
                labelParent.Name = "parent";
                labelLevel.Name = "level";
                labelFavorit.Name = "favorit";
                dropDown.Name = "dropDown";
                
                labelLevel.Text = level.ToString();
                labelParent.Visible = labelLevel.Visible = labelFavorit.Visible = false;
                p.Location = new Point(x, y);
                label.Location = new Point(x, Math.Abs(topLeftAnglePanel.Y - topLeftAngleLabel.Y));
                dropDown.Location = new Point(p.Size.Width - 20, label.Location.Y);
                label.Text = text;
                if (dropDownItems)
                    dropDown.Text = ">";
                else
                    dropDown.Text = "";
                p.Size = new Size(width, height);
                panel.Controls.Add(p);
                p.Controls.Add(label);
                p.Controls.Add(dropDown);
                    
                p.Controls.Add(labelParent);
                p.Controls.Add(labelLevel);
                p.Controls.Add(labelFavorit);
                switch (level)
                {
                    case 0:
                        {
                            if (maxWidthCMS < p.Width)
                                maxWidthCMS = p.Width;
                        }
                        break;
                    case 1:
                        {
                            if (maxWidthCMSlevel1 < p.Width)
                                maxWidthCMSlevel1 = p.Width;

                        }
                        break;
                    case 2:
                        {
                            if (maxWidthCMSlevel2 < p.Width)
                                maxWidthCMSlevel2 = p.Width;
                        }
                        break;
                    case 3:
                        {
                            if (maxWidthCMSlevel3 < p.Width)
                                maxWidthCMSlevel3 = p.Width;
                        }
                        break;
                    case 4:
                        {
                            if (maxWidthCMSlevel4 < p.Width)
                                maxWidthCMSlevel4 = p.Width;
                        }
                        break;

                }
                if (text != "-")
                {
                    dropDown.MouseHover += new EventHandler(labelDropDown_MouseHover);
                    dropDown.MouseLeave += new EventHandler(labelDropDown_MouseLeave);
                    dropDown.MouseDown += new MouseEventHandler(labelDropDown_MouseDown);
                    p.Enter += new EventHandler(pnl_Enter);
                    p.MouseDown += new MouseEventHandler(pnl_MouseDown);
                    p.MouseHover += new EventHandler(panel_MouseHover);
                    p.MouseLeave += new EventHandler(panel_MouseLeave);
                    p.BackColorChanged += new EventHandler(pnl_BackColorChanged);
                    label.MouseHover += new EventHandler(lbl_MouseHover);
                    label.MouseLeave += new EventHandler(lbl_MouseLeave);
                    label.MouseDown += new MouseEventHandler(lbl_MouseDown);
                    label.MouseUp += new MouseEventHandler(lbl_MouseUp);
                    label.Click += new EventHandler(labelItem_Click);
                    
                }
            }
        }

        private void pbMenuItem_Click(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                ((PictureBox)sender).BorderStyle = BorderStyle.FixedSingle;
                string name = "";
                string parent = "";
                int level = 0;
                if (sender is PictureBox)
                {
                    foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            if (((Label)obj).Visible == false)
                            {
                                if (((Label)obj).Name == "parent")
                                    parent = ((Label)obj).Text;
                                else if (((Label)obj).Name == "level")
                                    level = Convert.ToInt32(((Label)obj).Text);
                            }
                            else if (((Label)obj).Name == "name")
                            {
                                name = ((Label)obj).Text;
                            }
                        }
                        for (int i = 0; i < listItems.Count; i++)
                        {
                            if (name.TrimStart().ToLower() == listItems[i].name.ToLower() && parent == listItems[i].parent && level == listItems[i].level)
                            {
                                if (!hasChildren(listItems[i]))
                                {
                                    listItems[i].favorit = true;
                                    if (!isIn(allFavorites, listItems[i]))
                                    {
                                        allFavorites.Add(listItems[i]);
                                        richTextBoxLog.Text = sepLog + " Добавили в избранное пункт меню \"" + listItems[i].name + "\"\n" + richTextBoxLog.Text;
                                        if (currentVariant.Text == "Вариант 4")
                                        {
                                            clearMyPanel(panelSearch, 2);
                                            search(panelTextBox.Text.ToLower());
                                        }
                                        else
                                        {
                                            getFavoritesForPanel();
                                            clearMyPanel(myContextMenu, 0);
                                            if (panelTextBox.Text != "" && !favoritesShow)
                                            {
                                                search(panelTextBox.Text);
                                            }
                                            else
                                            {
                                                setMyContextMenu();
                                                setPanelSize(myContextMenu, defaultMyCMSSize.Width, defaultMyCMSSize.Height);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        richTextBoxLog.Text = sepLog + " Пункт меню \"" + listItems[i].name + "\" уже находится в избранном.\n" + richTextBoxLog.Text;
                                    } 
                                    name = parent = "";
                                    level = 0;
                                }
                            }
                        }
                    }
                }
            }
        }

        private void pbMenuItem_MouseHover(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                ((PictureBox)sender).Image = Image.FromFile(filenameFavoritSelect);
                ((PictureBox)sender).BorderStyle = BorderStyle.None;
                ((PictureBox)sender).Parent.BackColor = colorWhenMouseHover;
                foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                {
                    if (obj is Label)
                    {
                        ((Label)obj).BackColor = colorWhenMouseHover;
                        if (((Label)obj).Visible == false && ((Label)obj).Name == "favorit")
                        {
                            if (((Label)obj).Text == "favorit")
                            {
                                ((PictureBox)sender).Image = Image.FromFile(filenameDeleteSelect);
                                ((PictureBox)sender).Click += new EventHandler(pbDelete_Click);
                            }
                        }
                    }
                }
            }
        }

        private void pbMenuItem_MouseLeave(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                ((PictureBox)sender).BorderStyle = BorderStyle.None;
            }
        }

        int setMaxWidth(int maxKolWords, int width)
        {
            if (maxKolWords > maxKolChar)
                width += (int)((maxKolWords - maxKolChar) / 3);
            return width;
        }

        int panelHeight = 0;
        int panelWidth = 0;
        //добавляем ричтекстбоксы вместо лейблов
        void addMyItemRichTextBox(Panel panel, string text, Item item, string request, int x, int y,/*сдвиг вниз относительно верхнего соседа*/ int width, int height, int maxwidth, int maxkolwords)
        {
            try
            {
                //только в четвертом и шестом создаются пикчербоксы в результатах поиска
                if (currentVariant.Text == "Вариант 3")
                {
                    if (panel.Controls.Count == 3)
                    {
                        panelHeight = topLeftFirstItemPanelSearch.Y + height;
                        panelWidth = width;
                    }
                    width = maxwidth;
                    if (maxwidth > maxWidthMyContextMenu)
                        width = maxWidthMyContextMenu;
                    Panel p = new Panel();
                    RichTextBox rtb = new RichTextBox();
                    Label labelParent = new Label();
                    Label labelLevel = new Label();
                    Label labelName = new Label();
                    Label maxKolWords = new Label();
                    Label kolWords = new Label();
                    panel.Controls.Add(p);
                    p.Controls.Add(labelLevel);
                    p.Controls.Add(rtb);
                    p.Controls.Add(labelParent);
                    p.Controls.Add(labelName);
                    p.Controls.Add(maxKolWords);
                    p.Controls.Add(kolWords);
                    p.TabIndex = 1;

                    maxKolWords.Name = "maxKolWords";
                    maxKolWords.Visible = false;
                    maxKolWords.Text = maxkolwords.ToString();

                    kolWords.Visible = false;
                    kolWords.Name = "kolWords";

                    labelName.Text = item.name + " (";
                    for (int i = 0; i < item.listSynonims.Count; i++)
                        labelName.Text += item.listSynonims[i] + ", " ;
                    labelName.Text += ")";
                    if (labelName.Text.IndexOf(" ()") != -1)
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(" ()"));
                    if (labelName.Text.IndexOf(", )") != -1)
                    { 
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(", )"));
                        labelName.Text += ")";
                    }
                    labelName.Visible = false;
                    labelName.Name = "toolTipName";

                    labelLevel.Name = "level";
                    labelLevel.Visible = false;
                    labelLevel.Text = item.level.ToString();
                    
                    labelParent.Name = "parent";
                    labelParent.Text = item.parent;
                    labelParent.Visible = false;
                    p.Font = rtb.Font = font;
                    p.Location = new Point(x, y);
                    p.Size = new Size(width, height);

                    panelWidth = width;
                    panelHeight += height;

                    rtb.Size = new Size(maxwidth, height);
                    rtb.BorderStyle = BorderStyle.None;
                    rtb.Visible = true;
                    rtb.ReadOnly = true;
                    rtb.Multiline = false;
                    rtb.Cursor = Cursors.Default;
                    rtb.BackColor = colorDefault;
                    rtb.Location = new Point(3, 5);
                    string s = text.ToLower();
                    if (s.IndexOf(request) != -1)
                    {
                        rtb.AppendText(text + "\n");
                        int begin = s.IndexOf(request);
                        rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else if (isSynonimRequestOk(item, request))
                    {
                        string syn = text;
                        rtb.ForeColor = colorText;
                        syn += " (";
                        for (int i = 0; i < item.listSynonims.Count; i++)
                        {
                            string str = item.listSynonims[i];
                            
                            if (str.IndexOf(request) != -1 || item.listSynonims[i].IndexOf(request) != -1)
                            {
                                syn += item.listSynonims[i] + ",";
                            }
                        }
                        syn += ")";
                        if (syn.IndexOf(" ()") != -1)
                            syn = syn.Substring(0, syn.IndexOf(" ()"));
                        if (syn.IndexOf(",)") != -1)
                            syn = syn.Substring(0, syn.IndexOf(",)")) + ")"; 
                        rtb.AppendText(syn + "\n");
                        int begin = syn.ToLower().IndexOf(request);
                        if (begin != -1)
                            rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else 
                    {
                        rtb.Text = text;
                        rtb.ForeColor = colorNotEnabled;
                    }
                    if (text != "-")
                    {
                        p.MouseDown += new MouseEventHandler(pnl_MouseDown);
                        p.MouseHover += new EventHandler(panel_MouseHover);
                        p.MouseLeave += new EventHandler(panel_MouseLeave);
                        p.BackColorChanged += new EventHandler(pnl_BackColorChanged);
                        rtb.MouseHover += new EventHandler(lbl_MouseHover);
                        rtb.MouseLeave += new EventHandler(lbl_MouseLeave);
                        rtb.MouseDown += new MouseEventHandler(lbl_MouseDown);
                        rtb.MouseUp += new MouseEventHandler(lbl_MouseUp);
                        rtb.Click += new EventHandler(labelItem_Click);
                        
                        p.Enter += new EventHandler(panel_Enter);
                    }

                    kolWords.Text = rtb.Text.TrimStart().Length.ToString();

                    panel.Update();
                    setPanelSize(panelSearch, panelWidth, panelHeight);
                }
                else if (currentVariant.Text == "Вариант 4" )
                {
                    if (panel.Controls.Count == 3)
                    {
                        panelHeight = topLeftFirstItemPanelSearch.Y + height;
                        panelWidth = width;
                    }
                    width = maxwidth;
                    if (maxwidth > maxWidthMyContextMenu)
                        width = maxWidthMyContextMenu;
                    Panel p = new Panel();
                    RichTextBox rtb = new RichTextBox();
                    PictureBox pb = new PictureBox();
                    Label labelParent = new Label();
                    Label labelLevel = new Label();
                    Label labelFavorit = new Label();
                    Label labelName = new Label();
                    Label maxKolWords = new Label();
                    Label kolWords = new Label();
                    panel.Controls.Add(p);
                    p.Controls.Add(labelLevel);
                    p.Controls.Add(rtb);
                    p.Controls.Add(pb);
                    p.Controls.Add(labelParent);
                    p.Controls.Add(labelFavorit);
                    p.Controls.Add(labelName);
                    p.Controls.Add(maxKolWords);
                    p.Controls.Add(kolWords);
                    maxKolWords.Name = "maxKolWords";
                    maxKolWords.Visible = false;
                    maxKolWords.Text = maxkolwords.ToString();

                    kolWords.Visible = false;
                    kolWords.Name = "kolWords";

                    labelName.Text = item.name + " (";
                    for (int i = 0; i < item.listSynonims.Count; i++)
                        labelName.Text += item.listSynonims[i] + ", ";
                    labelName.Text += ")";
                    if (labelName.Text.IndexOf(" ()") != -1)
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(" ()"));
                    if (labelName.Text.IndexOf(", )") != -1)
                    {
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(", )"));
                        labelName.Text += ")";
                    }
                    labelName.Visible = false;
                    labelName.Name = "toolTipName";

                    labelFavorit.Name = "favorit";
                    labelFavorit.Visible = false;
                    if (item.favorit)
                        labelFavorit.Text = "favorit";
                    else
                        labelFavorit.Text = "not favorit";

                    labelLevel.Name = "level";
                    labelLevel.Visible = false;
                    labelLevel.Text = item.level.ToString();

                    labelParent.Name = "parent";
                    labelParent.Text = item.parent;
                    labelParent.Visible = false;

                    p.Font = rtb.Font = font;
                    p.Location = new Point(x, y);
                    p.Size = new Size(width, height);

                    panelWidth = width;
                    panelHeight += height;
                    pb.Size = new Size(16, 16);
                    pb.Visible = false;
                    rtb.Size = new Size(width - 2*pb.Width, height);
                    rtb.BorderStyle = BorderStyle.None;
                    rtb.Visible = true;
                    rtb.ReadOnly = true;
                    rtb.Multiline = false;
                    rtb.HideSelection = true;
                    rtb.Cursor = Cursors.Default;
                    rtb.BackColor = colorDefault;
                    rtb.Location = new Point(3, 5);
                    if (!hasChildren(item))
                    {
                        pb.Location = new Point(width - 2 * pb.Width, 5);
                        if (item.favorit)
                        {
                            pb.Image = Image.FromFile(filenameFavorit);
                        }
                        else
                        {
                            pb.Image = Image.FromFile(filenameFavoritFar);
                        }
                        pb.Visible = true;
                        pb.Click += new EventHandler(pbFavorit_Click);
                        pb.MouseHover += new EventHandler(pb_MouseHover);
                        pb.MouseLeave += new EventHandler(pb_MouseLeave);
                    }
                    string s = text.ToLower();
                    if (s.IndexOf(request) != -1)
                    {
                        rtb.AppendText(text + "\n");
                        int begin = s.IndexOf(request);
                        rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else if (isSynonimRequestOk(item, request))
                    {
                        string syn = text;
                        rtb.ForeColor = colorText;
                        syn += " (";
                        for (int i = 0; i < item.listSynonims.Count; i++)
                        {
                            string str = item.listSynonims[i];

                            if (str.IndexOf(request) != -1 || item.listSynonims[i].IndexOf(request) != -1)
                            {
                                syn += item.listSynonims[i] + ",";
                            }


                        }
                        syn += ")";
                        if (syn.IndexOf(" ()") != -1)
                            syn = syn.Substring(0, syn.IndexOf(" ()"));
                        if (syn.IndexOf(",)") != -1)
                            syn = syn.Substring(0, syn.IndexOf(",)")) + ")";
                        rtb.AppendText(syn + "\n");
                        int begin = syn.ToLower().IndexOf(request);
                        if (begin != -1)
                            rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else
                    {
                        rtb.Text = text;
                        rtb.ForeColor = colorNotEnabled;
                    }
                    if (text != "-")
                    {
                        p.MouseDown += new MouseEventHandler(pnl_MouseDown);
                        p.MouseHover += new EventHandler(panel_MouseHover);
                        p.MouseLeave += new EventHandler(panel_MouseLeave);
                        p.BackColorChanged += new EventHandler(pnl_BackColorChanged);
                        rtb.MouseHover += new EventHandler(lbl_MouseHover);
                        rtb.MouseLeave += new EventHandler(lbl_MouseLeave);
                        rtb.MouseDown += new MouseEventHandler(lbl_MouseDown);
                        rtb.MouseUp += new MouseEventHandler(lbl_MouseUp);
                        rtb.Click += new EventHandler(labelItem_Click);
                        
                    }

                    kolWords.Text = rtb.Text.TrimStart().Length.ToString();
                    panel.Update();
                    setPanelSize(panelSearch, panelWidth, panelHeight);
                    
                }
                else if (currentVariant.Text == "Вариант 5")
                {
                    if (panel.Controls.Count == 0)
                    {
                        panelHeight = topLeftFirstItemPanelSearch.Y + height;
                        panelWidth = 0;
                    }
                    width = maxwidth;
                    if (maxwidth > maxWidthMyContextMenu)
                        width = maxWidthMyContextMenu;
                    Panel p = new Panel();
                    RichTextBox rtb = new RichTextBox();
                    Label labelParent = new Label();
                    Label labelLevel = new Label();
                    Label labelName = new Label();
                    Label maxKolWords = new Label();
                    Label kolWords = new Label();
                    panel.Controls.Add(p);
                    p.Controls.Add(labelLevel);
                    p.Controls.Add(rtb);
                    p.Controls.Add(labelParent);
                    p.Controls.Add(labelName);
                    p.Controls.Add(maxKolWords);
                    p.Controls.Add(kolWords);
                    maxKolWords.Name = "maxKolWords";
                    maxKolWords.Visible = false;
                    maxKolWords.Text = maxkolwords.ToString();

                    kolWords.Visible = false;
                    kolWords.Name = "kolWords";

                    labelName.Text = item.name + " (";
                    for (int i = 0; i < item.listSynonims.Count; i++)
                        labelName.Text += item.listSynonims[i] + ", ";
                    labelName.Text += ")";
                    if (labelName.Text.IndexOf(" ()") != -1)
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(" ()"));
                    if (labelName.Text.IndexOf(", )") != -1)
                    {
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(", )"));
                        labelName.Text += ")";
                    }
                    labelName.Visible = false;
                    labelName.Name = "toolTipName";

                    labelLevel.Name = "level";
                    labelLevel.Visible = false;
                    labelLevel.Text = item.level.ToString();

                    labelParent.Name = "parent";
                    labelParent.Text = item.parent;
                    labelParent.Visible = false;

                    p.Font = rtb.Font = font;
                    p.Location = new Point(x, y);
                    p.Size = new Size(width, height);

                    panelWidth = width;
                    panelHeight += height;

                    rtb.Size = new Size(maxwidth, height);
                    rtb.BorderStyle = BorderStyle.None;
                    rtb.Visible = true;
                    rtb.ReadOnly = true;
                    rtb.Multiline = false;
                    rtb.Cursor = Cursors.Default;
                    rtb.BackColor = colorDefault;
                    rtb.Location = new Point(3, 5);
                    string s = text.ToLower();
                    if (s.IndexOf(request) != -1)
                    {
                        rtb.AppendText(text + "\n");
                        int begin = s.IndexOf(request);
                        rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else if (isSynonimRequestOk(item, request))
                    {
                        string syn = text;
                        rtb.ForeColor = colorText;
                        syn += " (";
                        for (int i = 0; i < item.listSynonims.Count; i++)
                        {
                            string str = item.listSynonims[i];

                            if (str.IndexOf(request) != -1 || item.listSynonims[i].IndexOf(request) != -1)
                            {
                                syn += item.listSynonims[i] + ",";
                            }


                        }
                        syn += ")";
                        if (syn.IndexOf(" ()") != -1)
                            syn = syn.Substring(0, syn.IndexOf(" ()"));
                        if (syn.IndexOf(",)") != -1)
                            syn = syn.Substring(0, syn.IndexOf(",)")) + ")";
                        rtb.AppendText(syn + "\n");
                        int begin = syn.ToLower().IndexOf(request);
                        if (begin != -1)
                            rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else
                    {
                        rtb.Text = text;
                        rtb.ForeColor = colorNotEnabled;
                    }
                    if (text != "-")
                    {
                        p.MouseDown += new MouseEventHandler(pnl_MouseDown);
                        p.MouseHover += new EventHandler(panel_MouseHover);
                        p.MouseLeave += new EventHandler(panel_MouseLeave);
                        p.BackColorChanged += new EventHandler(pnl_BackColorChanged);
                        rtb.MouseHover += new EventHandler(lbl_MouseHover);
                        rtb.MouseLeave += new EventHandler(lbl_MouseLeave);
                        rtb.MouseDown += new MouseEventHandler(lbl_MouseDown);
                        rtb.MouseUp += new MouseEventHandler(lbl_MouseUp);
                        rtb.Click += new EventHandler(labelItem_Click);
                    }
                    kolWords.Text = rtb.Text.TrimStart().Length.ToString();
                    panel.Update();
                    setPanelSize(myContextMenu, panelWidth, panelHeight);
                }
                else if (currentVariant.Text == "Вариант 6")
                {
                    if (panel.Controls.Count == 0)
                    {
                        panelHeight = topLeftFirstItemPanelSearch.Y + height;
                        panelWidth = 0;
                    }
                    width = maxwidth;
                    if (maxwidth > maxWidthMyContextMenu)
                        width = maxWidthMyContextMenu;
                    Panel p = new Panel();
                    RichTextBox rtb = new RichTextBox();
                    PictureBox pb = new PictureBox();
                    Label labelParent = new Label();
                    Label labelLevel = new Label();
                    Label labelName = new Label();
                    Label maxKolWords = new Label();
                    Label kolWords = new Label();
                    Label labelFavorit = new Label();
                    panel.Controls.Add(p);
                    p.Controls.Add(labelFavorit);
                    p.Controls.Add(labelParent);
                    p.Controls.Add(labelLevel);
                    p.Controls.Add(rtb);
                    p.Controls.Add(pb);
                    p.Controls.Add(labelName);
                    p.Controls.Add(maxKolWords);
                    p.Controls.Add(kolWords);
                    maxKolWords.Name = "maxKolWords";
                    maxKolWords.Visible = false;
                    maxKolWords.Text = maxkolwords.ToString();

                    kolWords.Visible = false;
                    kolWords.Name = "kolWords";

                    labelName.Text = item.name + " (";
                    for (int i = 0; i < item.listSynonims.Count; i++)
                        labelName.Text += item.listSynonims[i] + ", ";
                    labelName.Text += ")";
                    if (labelName.Text.IndexOf(" ()") != -1)
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(" ()"));
                    if (labelName.Text.IndexOf(", )") != -1)
                    {
                        labelName.Text = labelName.Text.Substring(0, labelName.Text.IndexOf(", )"));
                        labelName.Text += ")";
                    }
                    labelName.Visible = false;
                    labelName.Name = "toolTipName";
                    labelParent.Name = "parent";
                    labelLevel.Name = "level";
                    labelLevel.Visible = false;
                    labelLevel.Text = item.level.ToString();

                    labelFavorit.Name = "favorit";
                    labelFavorit.Visible = false;
                    if (item.favorit)
                        labelFavorit.Text = "favorit";
                    else
                        labelFavorit.Text = "not favorit";
                    
                    pb.Size = new Size(0, 0);
                    if (!hasChildren(item))
                    {
                        pb.Size = new Size(16, 16);
                        pb.Location = new Point(175, 4);
                        if (item.favorit)
                        {
                            pb.Image = Image.FromFile(filenameFavorit);
                        }
                        else
                        {
                            pb.Image = Image.FromFile(filenameFavoritFar);
                        }
                        pb.Visible = true;
                        pb.Click += new EventHandler(pbFavorit_Click);
                        pb.MouseHover += new EventHandler(pb_MouseHover);
                        pb.MouseLeave += new EventHandler(pb_MouseLeave);
                    }

                    labelParent.Text = item.parent;
                    labelParent.Visible = false;

                    p.Font = rtb.Font = font;
                    p.Location = new Point(x, y);
                    p.Size = new Size(width, height);

                    panelWidth = width;
                    panelHeight += height;

                    rtb.Size = new Size(maxwidth, height);
                    rtb.BorderStyle = BorderStyle.None;
                    rtb.Visible = true;
                    rtb.ReadOnly = true;
                    rtb.Multiline = false;
                    rtb.Cursor = Cursors.Default;
                    rtb.BackColor = colorDefault;
                    rtb.Location = new Point(3, 5);
                    rtb.Size = new Size(p.Width - 2 * pb.Width, rtb.Height);
                    string s = text.ToLower();
                    if (s.IndexOf(request) != -1)
                    {
                        rtb.AppendText(text + "\n");
                        int begin = s.IndexOf(request);
                        rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else if (isSynonimRequestOk(item, request))
                    {
                        string syn = text;
                        rtb.ForeColor = colorText;
                        syn += " (";
                        for (int i = 0; i < item.listSynonims.Count; i++)
                        {
                            string str = item.listSynonims[i];

                            if (str.IndexOf(request) != -1 || item.listSynonims[i].IndexOf(request) != -1)
                            {
                                syn += item.listSynonims[i] + ",";
                            }
                        }
                        syn += ")";
                        if (syn.IndexOf(" ()") != -1)
                            syn = syn.Substring(0, syn.IndexOf(" ()"));
                        if (syn.IndexOf(",)") != -1)
                            syn = syn.Substring(0, syn.IndexOf(",)")) + ")";
                        rtb.AppendText(syn + "\n");
                        int begin = syn.ToLower().IndexOf(request);
                        if (begin != -1)
                            rtb.Select(begin, request.Length);
                        rtb.SelectionFont = new Font(myFont, (float)myFontSize, FontStyle.Bold);
                    }
                    else
                    {
                        rtb.Text = text;
                        rtb.ForeColor = colorNotEnabled;
                    }
                    if (text != "-")
                    {
                        p.MouseDown += new MouseEventHandler(pnl_MouseDown);
                        p.MouseHover += new EventHandler(panel_MouseHover);
                        p.MouseLeave += new EventHandler(panel_MouseLeave);
                        p.BackColorChanged += new EventHandler(pnl_BackColorChanged);
                        rtb.MouseHover += new EventHandler(lbl_MouseHover);
                        rtb.MouseLeave += new EventHandler(lbl_MouseLeave);
                        rtb.MouseDown += new MouseEventHandler(lbl_MouseDown);
                        rtb.MouseUp += new MouseEventHandler(lbl_MouseUp);
                        rtb.Click += new EventHandler(labelItem_Click);
                    }
                    kolWords.Text = rtb.Text.TrimStart().Length.ToString();
                    panel.Update();
                    setPanelSize(myContextMenu, panelWidth, panelHeight);
                }
                
            }
            catch (Exception e)
            {
                MessageBox.Show("Ошибка в методе addMyItemRichTextBox\n" + e.StackTrace + "\n" + e.Message, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                logErorrs.Add("Ошибка в методе addMyItemRichTextBox\n" + e.StackTrace + "\n" + e.Message);
            }
        }

        //возвращает список детей данного пункта
        List<Item> getChildren(Item item)
        {
            List<Item> list = new List<Item>();
            for (int i = 0; i < listItems.Count; i++)
            {
                if (item.name == listItems[i].parent)
                {
                    list.Add(listItems[i]);
                }
            }
            return list;
        }

        bool hasChildren(Item item)
        {
            for (int i = 0; i < listItems.Count; i++)
            {
                if (item.name == listItems[i].parent)
                    return true;
            }
            return false;
        }

        //создает второстепенное окно и заполняет его результатами
        void addPanel(Item item, int coordX, int coordY, List<Item> list, int level)
        {
            
            //level - уровень панели, которая будет родителем для создаваемой, поэтому нужно увеличить уровень на один
            level++;
            int locX = 3;
            int locY = 4;
            int x = locX;
            int y = locY;
            int delta = pnl.Height;
            int width = pnl.Width;
            int deltainc = 0;
            int otstup = 5;
            int height = pnl.Height;
            bool wasSeparator = false;
            Panel p = panel1;
            switch (level)
            {
                case 1: { curentPanel = panel1; }
                    break;
                case 2: { p = panel2;
                curentPanel = panel2;
                }
                    break;
                case 3: { p = panel3;
                curentPanel = panel3;
                }
                    break;
                case 4: { p = panel4;
                curentPanel = panel4;
                }
                    break;
            }
            //очищаем панель от предыдущих записей
            clearMyPanel(p,0);
            //curentPanelLocation = new Point(curentPanel.Location.X, curentPanel.Location.Y);
            p.Size = new System.Drawing.Size(width, delta);
            //this.listBoxDynamic.Controls.Add(p);
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].level == level && item.name == list[i].parent)
                {
                    if (list[i].name != "-")
                    {
                        if (hasChildren(list[i]))
                            addMyItem(p, list[i], list[i].name, list[i].parent, list[i].favorit, true, x, deltainc + otstup, width, height, level);
                        else
                            addMyItem(p, list[i], list[i].name, list[i].parent, list[i].favorit, false, x, deltainc + otstup, width, height, level);
                        deltainc += delta;
                        switch (level)
                        {
                            case 0:
                                {
                                    maxHeightCMS += delta + otstup;
                                }
                                break;
                            case 1:
                                {
                                    maxHeightCMSlevel1 += delta + otstup;
                                }
                                break;
                            case 2:
                                {
                                    maxHeightCMSlevel2 += delta + otstup;
                                }
                                break;
                            case 3:
                                {
                                    maxHeightCMSlevel3 += delta + otstup;
                                }
                                break;
                            case 4:
                                {
                                    maxHeightCMSlevel4 += delta + otstup;
                                }
                                break;
                        }
                    }
                    else
                    {
                        if (hasChildren(listItems[i]))
                            addMyItem(p, list[i], list[i].name, list[i].parent, list[i].favorit, true, x, deltainc + otstup, width, height, level);
                        else
                            addMyItem(p, list[i], list[i].name, list[i].parent, list[i].favorit, false, x, deltainc + otstup, width, height, level);
                        if (!wasSeparator)
                            wasSeparator = true;
                        else
                            wasSeparator = false;
                        deltainc += delta / 2;
                        switch (level)
                        {
                            case 0:
                                {
                                    maxHeightCMS += delta / 2;
                                }
                                break;
                            case 1:
                                {
                                    maxHeightCMSlevel1 += delta / 2;
                                }
                                break;
                            case 2:
                                {
                                    maxHeightCMSlevel2 += delta / 2;
                                }
                                break;
                            case 3:
                                {
                                    maxHeightCMSlevel3 += delta / 2;
                                }
                                break;
                            case 4:
                                {
                                    maxHeightCMSlevel4 += delta / 2;
                                }
                                break;
                        }
                    }
                }
                    
            }
            switch (level)
            {
                case 0:
                    {
                        if (p.Size.Height < maxHeightCMS)
                            p.Size = new Size(maxWidthCMS, maxHeightCMS);
                        else
                            p.Size = new Size(maxWidthCMS, p.Size.Height);
                    }
                    break;
                case 1:
                    {
                        if (p.Size.Height < maxHeightCMSlevel1)
                            p.Size = new Size(maxWidthCMSlevel1, maxHeightCMSlevel1);
                        else
                            p.Size = new Size(maxWidthCMSlevel1, p.Size.Height);
                    }
                    break;
                case 2:
                    {
                        if (p.Size.Height < maxHeightCMSlevel2)
                            p.Size = new Size(maxWidthCMSlevel2, maxHeightCMSlevel2);
                        else
                            p.Size = new Size(maxWidthCMSlevel2, p.Size.Height);
                    }
                    break;
                case 3:
                    {
                        if (p.Size.Height < maxHeightCMSlevel3)
                            p.Size = new Size(maxWidthCMSlevel3, maxHeightCMSlevel3);
                        else
                            p.Size = new Size(maxWidthCMSlevel3, p.Size.Height);
                    }
                    break;
                case 4:
                    {
                        if (p.Size.Height < maxHeightCMSlevel4)
                            p.Size = new Size(maxWidthCMSlevel4, maxHeightCMSlevel4);
                        else
                            p.Size = new Size(maxWidthCMSlevel4, p.Size.Height);
                    }
                    break;
            }
            maxHeightCMSlevel1 = 0;
            maxHeightCMSlevel2 = 0;
            maxHeightCMSlevel3 = 0;
            maxHeightCMSlevel4 = 0;

            p.Location = new Point(coordX, coordY);
            p.Visible = true;
        }




        private void lbl_MouseHover(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                ((Label)sender).BackColor = colorWhenMouseHover;
                ((Label)sender).Parent.BackColor = colorWhenMouseHover;
                foreach (Object obj in ((Label)sender).Parent.Controls)
                {
                    if (obj is Label)
                    {

                        ((Label)obj).BackColor = colorWhenMouseHover;

                    }
                    if (obj is PictureBox)
                    {
                        if (currentVariant.Text == "Вариант 4")
                        {
                            bool favorit = false;

                            foreach (Object ob in ((Label)sender).Parent.Controls)
                            {
                                if (ob is Label)
                                {
                                    if (((Label)ob).Visible == false)
                                        if (((Label)ob).Name == "favorit")
                                            if (((Label)ob).Text == "favorit")
                                                favorit = true;
                                }
                            }
                            if (favorit)
                                ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFarSelect);
                            else
                                ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFarSelect);

                        }
                        else if (currentVariant.Text == "Вариант 5")
                        {
                            ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFarSelect);
                        }
                        else if (currentVariant.Text == "Вариант 6")
                            if (((Label)sender).Parent.Parent.Name != "panelSearch")
                            {
                                bool favorit = false;
                                foreach (Object ob in ((Label)sender).Parent.Controls)
                                {
                                    if (ob is Label)
                                    {
                                        if (((Label)ob).Visible == false)
                                            if (((Label)ob).Name == "favorit")
                                                if (((Label)ob).Text == "favorit")
                                                    favorit = true;
                                    }
                                }
                                if (favorit)
                                    ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFarSelect);
                                else
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFarSelect);
                            }
                            else ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFarSelect);
                    }
                }
                if (((Label)sender).Parent.Parent.Name == "panelSearch")
                {
                    curentPanel = (Panel)((Label)sender).Parent.Parent;
                    curentPanelLocation = new Point(curentPanel.Location.X, curentPanel.Location.Y);
                    currentClickItemX = ((Label)sender).Parent.Location.X;
                    currentClickItemY = ((Label)sender).Parent.Location.Y;
                    int panelWidth = ((Label)sender).Parent.Width;
                    int X = curentPanelLocation.X + currentClickItemX + panelWidth - 4;
                    int Y = curentPanelLocation.Y + currentClickItemY;
                    foreach (Object obj in ((Label)sender).Parent.Controls)
                    { 
                        if (obj is Label)
                            if (((Label)obj).Name == "parentLine")
                                if (((Label)obj).Text != "")
                                    viewToolTip(((Label)obj).Text + "\\"+ ((Label)sender).Text, X + 5, Y, true);
                    }
                    
                    
                }
                //if (currentVariant.Text == "Вариант 6")
                //{
                //    bool favorit = false;
                //    foreach (Object obj in ((Label)sender).Parent.Controls)
                //    {
                //        if (obj is Label)
                //        {
                //            if (((Label)obj).Visible == false && ((Label)obj).Name == "favorit")
                //            {
                //                if (((Label)obj).Text == "favorit")
                //                {
                //                    favorit = true;
                //                }
                //                else
                //                    favorit = false;
                //                foreach (Object ob in ((Label)sender).Parent.Controls)
                //                    if (ob is PictureBox)
                //                        if (((Label)sender).Parent.Parent.Name != "panelSearch")
                //                            if (favorit)
                //                                ((PictureBox)ob).Image = Image.FromFile(filenameFavoritSelect);
                //                            else
                //                                ((PictureBox)ob).Image = Image.FromFile(filenameFavoritFarSelect);
                //            }
                //        }
                //    }

                //}
            }
            else if (sender is RichTextBox)
            {
                ((RichTextBox)sender).BackColor = colorWhenMouseHover;
                ((RichTextBox)sender).Parent.BackColor = colorWhenMouseHover;
                int rtbWidth = 0;
                int pWidth = 0;
                foreach (Object ob in ((RichTextBox)sender).Parent.Controls)
                {
                    if (ob is Label)
                    {
                        if (((Label)ob).Visible == false)
                        {
                            if (((Label)ob).Name == "maxKolWords")
                                pWidth = Convert.ToInt32(((Label)ob).Text);
                            else if (((Label)ob).Name == "kolWords")
                                rtbWidth = Convert.ToInt32(((Label)ob).Text);
                        }
                    }
                }

                if (currentVariant.Text == "Вариант 3")
                {
                    
                    if (rtbWidth > maxKolChar)
                        foreach (Object ob in ((RichTextBox)sender).Parent.Controls)
                        {
                            if (ob is Label)
                            {
                                if (((Label)ob).Visible == false)
                                    if (((Label)ob).Name == "toolTipName")
                                    {
                                        int x = ((RichTextBox)sender).Parent.Parent.Location.X + ((RichTextBox)sender).Parent.Width;
                                        int y = ((RichTextBox)sender).Parent.Parent.Location.Y + ((RichTextBox)sender).Parent.Location.Y + (int)((RichTextBox)sender).Parent.Height / 3;
                                        viewToolTip(((Label)ob).Text, x, y, true);
                                    }

                            }
                        }
                }
                else if (currentVariant.Text == "Вариант 4")
                {
                    //viewImage((Panel)((RichTextBox)sender).Parent);
                    bool favorit = false;
                    foreach (Object ob in ((RichTextBox)sender).Parent.Controls)
                    {
                        if (ob is Label)
                        {
                            if (((Label)ob).Visible == false)
                                if (((Label)ob).Name == "favorit")
                                {
                                    if (((Label)ob).Text == "favorit")
                                        favorit = true;
                                }
                            if (rtbWidth > maxKolChar)
                                if (((Label)ob).Name == "toolTipName")
                                {
                                    int x = ((RichTextBox)sender).Parent.Parent.Location.X + ((RichTextBox)sender).Parent.Width;
                                    int y = ((RichTextBox)sender).Parent.Parent.Location.Y + ((RichTextBox)sender).Parent.Location.Y + (int)((RichTextBox)sender).Parent.Height / 3;
                                    viewToolTip(((Label)ob).Text, x, y, true);
                                }

                        }
                    }
                    foreach (Object obj in ((RichTextBox)sender).Parent.Controls)
                    {
                        if (obj is PictureBox)
                            if (favorit)
                                ((PictureBox)obj).Image = Image.FromFile(filenameFavoritSelect);
                            else
                                ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFarSelect);
                    }
                }
                else if (currentVariant.Text == "Вариант 5")
                {
                    foreach (Object ob in ((RichTextBox)sender).Parent.Controls)
                    {
                        if (ob is Label)
                        {
                            if (((Label)ob).Visible == false)
                                if (rtbWidth > maxKolChar)
                                    if (((Label)ob).Name == "toolTipName")
                                    {
                                        labelToolTip.Text = ((Label)ob).Text;
                                        int x = ((RichTextBox)sender).Parent.Parent.Location.X - labelToolTip.Width - 7;
                                        int y = ((RichTextBox)sender).Parent.Parent.Location.Y + ((RichTextBox)sender).Parent.Location.Y + (int)((RichTextBox)sender).Parent.Height / 3;
                                        viewToolTip(((Label)ob).Text, x, y, false);
                                    }
                        }
                    }
                }
                else if (currentVariant.Text == "Вариант 6")
                {
                    foreach (Object obj in ((RichTextBox)sender).Parent.Controls)
                    {
                        if (obj is PictureBox)
                        {
                            if (!favoritesShow)
                            {
                                bool favorit = false;
                                foreach (Object ob in ((RichTextBox)sender).Parent.Controls)
                                {
                                    if (ob is Label)
                                    {
                                        if (((Label)ob).Visible == false)
                                            if (((Label)ob).Name == "favorit")
                                            {
                                                if (((Label)ob).Text == "favorit")
                                                    favorit = true;
                                            }
                                        if (rtbWidth > maxKolChar)
                                            if (((Label)ob).Name == "toolTipName")
                                            {
                                                labelToolTip.Text = ((Label)ob).Text;
                                                int x = ((RichTextBox)sender).Parent.Parent.Location.X - labelToolTip.Width - 7;
                                                int y = ((RichTextBox)sender).Parent.Parent.Location.Y + ((RichTextBox)sender).Parent.Location.Y + (int)((RichTextBox)sender).Parent.Height / 3;
                                                viewToolTip(((Label)ob).Text, x, y, false);
                                            }
                                    }
                                }

                                if (favorit)
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritSelect);
                                else
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFarSelect);
                            }
                            else
                                ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFarSelect);
                        }
                    }
                }
            }
        }

        void viewToolTip(string text, int oX, int oY, bool right)
        {
            labelToolTip.Visible = true;
            int i = text.IndexOf("|");
            text = text.Substring(i + 1, text.Length);
            if (right)
                labelToolTip.Text = "< " + text;
            else
                labelToolTip.Text = text + " >";
            labelToolTip.Location = new Point(oX, oY);
        }

        void viewToolTipSynonim(Panel p, Item i,string request, int oX, int oY)
        {
            Label l = new Label();
            l.Name = "toolTipSynonim";
            
            string text = i.name;
            for (int j = 0; j < i.listSynonims.Count; j++)
            {
                string s = i.listSynonims[j].ToLower();
                if (s.IndexOf(request) != -1 || i.listSynonims[j].IndexOf(request) != -1)
                    text += " " + i.listSynonims[j]; 
            }
            l.Visible = true;
            l.Text = "<" + text + "(синонимы данного пункта)";
            l.Location = new Point(oX, oY);
            l.BackColor = Color.Yellow;
            p.Controls.Add(l);
        }

        private void lbl_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (sender is Label)
                {
                    ((Label)sender).BackColor = colorDefault;
                    ((Label)sender).Parent.BackColor = colorDefault;
                    foreach (Object obj in ((Label)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            ((Label)obj).BackColor = colorDefault;
                        }
                        if (obj is PictureBox)
                        {
                            if (currentVariant.Text == "Вариант 4")
                            {
                                bool favorit = false;
                                foreach (Object ob in ((Label)sender).Parent.Controls)
                                {
                                    if (ob is Label)
                                    {
                                        if (((Label)ob).Visible == false)
                                            if (((Label)ob).Name == "favorit")
                                                if (((Label)ob).Text == "favorit")
                                                    favorit = true;
                                    }
                                }
                                if (favorit)
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavorit);
                                else
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFar);

                            }
                            else if (currentVariant.Text == "Вариант 5")
                            {
                                ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFar);
                            }
                            else if (currentVariant.Text == "Вариант 6")
                            //((PictureBox)obj).Image = Image.FromFile(filenameFavorit);
                            {
                                bool favorit = false;
                                foreach (Object ob in ((Label)sender).Parent.Controls)
                                {
                                    if (ob is Label)
                                    {
                                        if (((Label)ob).Visible == false && ((Label)ob).Name == "favorit")
                                        {
                                            if (((Label)ob).Text == "favorit")
                                            {
                                                favorit = true;
                                            }
                                            else
                                                favorit = false;
                                            foreach (Object o in ((Label)sender).Parent.Controls)
                                                if (o is PictureBox)
                                                    //if (((Label)sender).Parent.Parent.Name == "panelSearch")
                                                        if (favorit)
                                                            ((PictureBox)o).Image = Image.FromFile(filenameFavorit);
                                                        else
                                                            ((PictureBox)o).Image = Image.FromFile(filenameFavoritFar);
                                                    //else
                                                    //{
                                                    //    if (favorit)
                                                    //        ((PictureBox)o).Image = Image.FromFile(filenameFavorit);
                                                    //    else
                                                    //        ((PictureBox)o).Image = Image.FromFile(filenameFavoritFar);
                                                    //}
                                        }
                                    }
                                }
                            }
                        }
                        labelToolTip.Visible = false;
                    }
                    //if (currentVariant.Text == "Вариант 6")
                    //{
                    //    bool favorit = false;
                    //    foreach (Object obj in ((Label)sender).Parent.Controls)
                    //    {
                    //        if (obj is Label)
                    //        {
                    //            if (((Label)obj).Visible == false && ((Label)obj).Name == "favorit")
                    //            {
                    //                if (((Label)obj).Text == "favorit")
                    //                {
                    //                    favorit = true;
                    //                }
                    //                else
                    //                    favorit = false;
                    //                foreach (Object ob in ((Label)sender).Parent.Controls)
                    //                    if (ob is PictureBox)
                    //                        if (((Label)sender).Parent.Parent.Name != "panelSearch")
                    //                            if (favorit)
                    //                                ((PictureBox)ob).Image = Image.FromFile(filenameFavorit);
                    //                            else
                    //                                ((PictureBox)ob).Image = Image.FromFile(filenameFavoritFar);
                    //            }
                    //        }
                    //    }

                    //}
                }
                else if (sender is RichTextBox)
                {
                    ((RichTextBox)sender).BackColor = colorDefault;
                    ((RichTextBox)sender).Parent.BackColor = colorDefault;
                    if (currentVariant.Text == "Вариант 4")
                    {
                        //viewImage((Panel)((RichTextBox)sender).Parent);
                        bool favorit = false;
                        foreach (Object ob in ((RichTextBox)sender).Parent.Controls)
                        {
                            if (ob is Label)
                            {
                                if (((Label)ob).Visible == false)
                                    if (((Label)ob).Name == "favorit")
                                        if (((Label)ob).Text == "favorit")
                                            favorit = true;
                            }
                        }
                        foreach (Object obj in ((RichTextBox)sender).Parent.Controls)
                        {
                            if (obj is PictureBox)
                                if (favorit)
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavorit);
                                else
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFar);
                        }
                    }
                    else if (currentVariant.Text == "Вариант 6")
                    {
                        foreach (Object obj in ((RichTextBox)sender).Parent.Controls)
                        {
                            if (obj is PictureBox)
                            {
                                if (!favoritesShow)
                                {
                                    bool favorit = false;
                                    foreach (Object ob in ((RichTextBox)sender).Parent.Controls)
                                    {
                                        if (ob is Label)
                                        {
                                            if (((Label)ob).Visible == false)
                                                if (((Label)ob).Name == "favorit")
                                                    if (((Label)ob).Text == "favorit")
                                                        favorit = true;
                                        }
                                    }

                                    if (favorit)
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavorit);
                                    else
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFar);
                                }
                                else
                                    ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFar);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logErorrs.Add(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void pnl_BackColorChanged(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                if (((Panel)sender).BackColor == colorWhenMouseHover || ((Panel)sender).BackColor == colorClick)
                    ((Panel)sender).BorderStyle = BorderStyle.FixedSingle;
                else
                    ((Panel)sender).BorderStyle = BorderStyle.None;
            }
        }

        private void pnl_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Panel)
            {
                ((Panel)sender).BackColor = colorClick;
                ((Panel)sender).BorderStyle = BorderStyle.FixedSingle;
                if (currentVariant.Text == "Вариант 3")
                {


                    foreach (Object obj in ((Panel)sender).Controls)
                    {
                        if (obj is Label)
                        {
                            if (((Label)obj).Name == "name")
                            {
                                ((Label)obj).BackColor = colorClick;
                                richTextBoxLog.Text = sepLog + "\"" + ((Label)obj).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text;
                            }
                        }
                        if (obj is RichTextBox)
                        {
                            ((RichTextBox)obj).BackColor = colorClick;
                            foreach (Item item in listItems)
                            {
                                if (((RichTextBox)obj).Text == item.name)
                                {
                                    if (!hasChildren(item))
                                    {
                                        item.incClicks();
                                        richTextBoxLog.Text = sepLog + "\"" + ((RichTextBox)obj).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                foreach (Object obj in ((Panel)sender).Controls)
                {
                    if (obj is Label)
                    {
                        if (((Label)obj).Name == "dropDown" && ((Label)obj).Text == ">" || ((Label)obj).Name == "dropDown" && ((Label)obj).Text == "")
                        {
                            //определить уровень, на котором находимся
                            //закрыть все уровни ниже
                            switch (((Label)obj).Parent.Parent.Name)
                            {
                                case "myContextMenu":
                                    {
                                        panel1.Visible = false;
                                        panel2.Visible = false;
                                        panel3.Visible = false;
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel1":
                                    {
                                        panel2.Visible = false;
                                        panel3.Visible = false;
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel2":
                                    {
                                        panel3.Visible = false;
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel3":
                                    {
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel4": { }
                                    break;
                            }
                        }
                        if (((Label)obj).Name == "dropDown" && ((Label)obj).Text != ">")
                        {
                            foreach (Object ob in ((Panel)sender).Controls)
                            {
                                if (ob is Label)
                                    if (((Label)ob).Name == "name")
                                    {
                                        foreach (Item item in listItems)
                                        {
                                            if (((Label)ob).Text == item.name)
                                            {
                                                if (!hasChildren(item))
                                                {
                                                    item.incClicks();
                                                    richTextBoxLog.Text = sepLog + "\"" + ((Label)ob).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text;
                                                    panelSearch.Visible = false;
                                                    myContextMenu.Visible = false;
                                                }
                                            }
                                        }
                                    }
                            }
                        }
                        ((Label)obj).BackColor = colorClick;
                    }
                    if (obj is RichTextBox)
                    {
                        ((RichTextBox)obj).BackColor = colorClick;
                        foreach (Item item in listItems)
                        {
                            if (((RichTextBox)obj).Text == item.name)
                            {
                                if (!hasChildren(item))
                                {
                                    item.incClicks();
                                    richTextBoxLog.Text = sepLog + "\"" + ((RichTextBox)obj).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text;
                                }
                            }
                        }
                    }
                }
                foreach (Object obj in ((Panel)sender).Controls)
                {
                    if (obj is Label)
                        //открываешь dropDown
                        if (((Label)obj).Text == ">")
                        {
                            Label neighbor = getLabelNeighbor(((Label)obj));
                            curentPanel = (Panel)((Panel)sender).Parent;
                            curentPanelLocation = new Point(curentPanel.Location.X, curentPanel.Location.Y);
                            currentClickItemX = ((Label)obj).Parent.Location.X;
                            currentClickItemY = ((Label)obj).Parent.Location.Y;
                            int panelWidth = ((Label)obj).Parent.Width;
                            for (int i = 0; i < listItems.Count; i++)
                            {

                                if (neighbor.Text.IndexOf(listItems[i].name) != -1)
                                {
                                    if (getChildren(listItems[i]).Count != 0)
                                    {
                                        addPanel(listItems[i], curentPanelLocation.X + currentClickItemX + panelWidth - 4, curentPanelLocation.Y + currentClickItemY, getChildren(listItems[i]), listItems[i].level);
                                        if (curentPanel.Name != "myContextMenu")
                                        {
                                            switch (curentPanel.Name)
                                            {
                                                case "panel1":
                                                    {
                                                        //уровень не поменялся, но пункт выбран другой
                                                        if (oldPanelLevel == 1 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel2.Visible = false;
                                                            panel3.Visible = false;
                                                            panel4.Visible = false;
                                                        }

                                                        oldPanelLevel = 1;

                                                    }
                                                    break;
                                                case "panel2":
                                                    {
                                                        //уровень не поменялся, но пункт выбран другой
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel3.Visible = false;
                                                            panel4.Visible = false;
                                                        }
                                                        oldPanelLevel = 2;
                                                    }
                                                    break;
                                                case "panel3":
                                                    {
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel4.Visible = false;
                                                        }
                                                        oldPanelLevel = 3;
                                                    }
                                                    break;
                                                case "panel4":
                                                    {
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                        }
                                                        oldPanelLevel = 4;
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            panel1.Visible = false;
                                            panel2.Visible = false;
                                            panel3.Visible = false;
                                            panel4.Visible = false;
                                            panelSearch.Visible = false;
                                            oldPanelLevel = 0;
                                        }
                                    }
                                }
                            }
                            oldPanelLabel = neighbor.Text;

                        }
                    }
                }
            }

            
        }

        private void panel_MouseHover(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                ((Panel)sender).BackColor = colorWhenMouseHover;
                ((Panel)sender).BorderStyle = BorderStyle.FixedSingle;
                foreach (Object obj in ((Panel)sender).Controls)
                {
                    if (obj is Label)
                    {
                        ((Label)obj).BackColor = colorWhenMouseHover;
                    }
                    if (obj is RichTextBox)
                    {
                        ((RichTextBox)obj).BackColor = colorWhenMouseHover;
                    }
                    if (obj is PictureBox)
                    {
                        if (currentVariant.Text == "Вариант 4")
                        {
                            bool favorit = false;
                            foreach (Object ob in ((Panel)sender).Controls)
                            {
                                if (ob is Label)
                                {
                                    if (((Label)ob).Visible == false)
                                        if (((Label)ob).Name == "favorit")
                                            if (((Label)ob).Text == "favorit")
                                                favorit = true;
                                }
                            }
                            if (favorit)
                                ((PictureBox)obj).Image = Image.FromFile(filenameFavoritSelect);
                        }
                        else if (currentVariant.Text == "Вариант 5")
                        {
                            ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFarSelect);
                        }
                        else if (currentVariant.Text == "Вариант 6")
                        {
                            if (!favoritesShow)
                            {
                                bool favorit = false;
                                foreach (Object ob in ((Panel)sender).Controls)
                                {
                                    if (ob is Label)
                                    {
                                        if (((Label)ob).Visible == false)
                                            if (((Label)ob).Name == "favorit")
                                                if (((Label)ob).Text == "favorit")
                                                    favorit = true;
                                    }
                                }
                                if (favorit)
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritSelect);
                                else
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFarSelect);
                            }
                            else
                            {
                                bool favorit = false;
                                foreach (Object ob in ((Panel)sender).Controls)
                                {
                                    if (ob is Label)
                                    {
                                        if (((Label)ob).Visible == false)
                                            if (((Label)ob).Name == "favorit")
                                                if (((Label)ob).Text == "favorit")
                                                    favorit = true;
                                    }
                                }
                                if (favorit)
                                    ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFarSelect);
                                else
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFarSelect);
                            }
                        }
                    }
                }
                if (currentVariant.Text == "Вариант 4" )
                    //viewImage(((Panel)sender));
                if (currentVariant.Text == "Вариант 6")
                {
                    bool favorit = false;
                    foreach (Object obj in ((Panel)sender).Controls)
                    {
                        if (obj is Label)
                        {
                            if (((Label)obj).Visible == false && ((Label)obj).Name == "favorit")
                            {
                                if (((Label)obj).Text == "favorit")
                                {
                                    favorit = true;
                                }
                                else
                                    favorit = false;
                                foreach (Object ob in ((Panel)sender).Controls)
                                    if (ob is PictureBox)
                                        if (((Panel)sender).Parent.Name != "panelSearch")
                                            if (favorit)
                                                ((PictureBox)ob).Image = Image.FromFile(filenameFavoritSelect);
                                            else
                                                ((PictureBox)ob).Image = Image.FromFile(filenameFavoritFarSelect);
                            }
                        }
                        
                    }

                }
            }
        }

        //показывает картинку рядом с выбранным пунктом меню
        void viewImage(Panel p)
        {
            string name = "";
            string parent = "";
            int level = 0;
            foreach (Object obj in p.Controls)
            {
                if (obj is Label)
                {
                    
                    if (((Label)obj).Name == "parent")
                    {
                        parent = ((Label)obj).Text;
                    }
                    else if (((Label)obj).Name == "level")
                    {
                        level = Convert.ToInt32(((Label)obj).Text);
                    }
                }
                else if (obj is RichTextBox)
                {
                    name = ((RichTextBox)obj).Text.ToLower().TrimStart();
                }
            }
            for (int i = 0; i < listItems.Count; i++)
            {
                if (name == listItems[i].name.ToLower() && parent == listItems[i].parent && level == listItems[i].level)
                {
                    if (!hasChildren(listItems[i]))
                    {
                        PictureBox pb = new PictureBox();
                        p.Controls.Add(pb);
                        pb.Name = "addToFavoritesImage";
                        pb.Size = new Size(16, 16);
                        pb.Image = Image.FromFile(filenameFavoritFarSelect);
                        if (currentVariant.Text == "Вариант 4")
                            pb.Location = new Point(125, 5);
                        else if (currentVariant.Text == "Вариант 6")
                            pb.Location = new Point(160, 4);
                        pb.Click += new EventHandler(pbFavorit_Click);
                        pb.MouseHover += new EventHandler(pb_MouseHover);
                        pb.MouseLeave += new EventHandler(pb_MouseLeave);
                    }
                }
            }
            

        }
        //прячет картинку рядом с выбранным пунктом меню
        void hideImage(Panel p)
        {
            string name = "";
            string parent = "";
            int level = 0;
            foreach (Object obj in p.Controls)
            {
                if (obj is Label)
                {

                    if (((Label)obj).Name == "parent")
                    {
                        parent = ((Label)obj).Text;
                    }
                    else if (((Label)obj).Name == "level")
                    {
                        level = Convert.ToInt32(((Label)obj).Text);
                    }
                }
                else if (obj is RichTextBox)
                {
                    name = ((RichTextBox)obj).Text.ToLower().TrimStart();
                }
            }
            for (int i = 0; i < listItems.Count; i++)
            {
                if (name == listItems[i].name.ToLower() && parent == listItems[i].parent && level == listItems[i].level)
                {
                    if (!hasChildren(listItems[i]))
                    {
                        foreach (Object ob in p.Controls)
                        {
                            if (ob is PictureBox)
                            {
                                if (((PictureBox)ob).Name == "addToFavoritesImage")
                                    p.Controls.Remove(((PictureBox)ob));
                            }
                        }
                    }
                }
            }
            
        }

        private void pb_MouseHover(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                if (currentVariant.Text == "Вариант 4")
                {
                    bool favorit = false;
                    foreach (Object ob in ((PictureBox)sender).Parent.Controls)
                    {
                        if (ob is Label)
                        {
                            if (((Label)ob).Visible == false)
                                if (((Label)ob).Name == "favorit")
                                    if (((Label)ob).Text == "favorit")
                                        favorit = true;
                        }
                    }
                    if (favorit)
                        ((PictureBox)sender).Image = Image.FromFile(filenameDeleteSelect);
                    else
                        ((PictureBox)sender).Image = Image.FromFile(filenameFavoritSelect);
                }
                else if (currentVariant.Text == "Вариант 5")
                {
                    ((PictureBox)sender).Image = Image.FromFile(filenameDeleteSelect);
                }
                else if (currentVariant.Text == "Вариант 6")
                {
                    if (!favoritesShow)
                    {
                        bool favorit = false;
                        foreach (Object ob in ((PictureBox)sender).Parent.Controls)
                        {
                            if (ob is Label)
                            {
                                if (((Label)ob).Visible == false)
                                    if (((Label)ob).Name == "favorit")
                                        if (((Label)ob).Text == "favorit")
                                            favorit = true;
                            }
                        }
                        if (favorit)
                            ((PictureBox)sender).Image = Image.FromFile(filenameDeleteSelect);
                        else
                            ((PictureBox)sender).Image = Image.FromFile(filenameFavoritSelect);
                    }
                    else
                        ((PictureBox)sender).Image = Image.FromFile(filenameDeleteSelect);
                }
                ((PictureBox)sender).BorderStyle = BorderStyle.None;
                ((PictureBox)sender).Parent.BackColor = colorWhenMouseHover;
                foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                {
                    if (obj is RichTextBox)
                    {
                        ((RichTextBox)obj).BackColor = colorWhenMouseHover;
                    }
                    if (obj is Label)
                    {
                        ((Label)obj).BackColor = colorWhenMouseHover;
                    }
                }
            }
        }

        private void pb_MouseLeave(object sender, EventArgs e)
        {
            if (sender is PictureBox)
            {
                if (currentVariant.Text == "Вариант 4")
                {
                    bool favorit = false;
                    foreach (Object ob in ((PictureBox)sender).Parent.Controls)
                    {
                        if (ob is Label)
                        {
                            if (((Label)ob).Visible == false)
                                if (((Label)ob).Name == "favorit")
                                    if (((Label)ob).Text == "favorit")
                                        favorit = true;
                        }
                    }
                    if (favorit)
                        ((PictureBox)sender).Image = Image.FromFile(filenameFavorit);
                    else
                        ((PictureBox)sender).Image = Image.FromFile(filenameFavoritFar);
                }
                else if (currentVariant.Text == "Вариант 5")
                    ((PictureBox)sender).Image = Image.FromFile(filenameDeleteFar);
                else if (currentVariant.Text == "Вариант 6")
                {
                    bool favorit = false;
                    foreach (Object ob in ((PictureBox)sender).Parent.Controls)
                    {
                        if (ob is Label)
                        {
                            if (((Label)ob).Visible == false)
                                if (((Label)ob).Name == "favorit")
                                    if (((Label)ob).Text == "favorit")
                                        favorit = true;
                        }
                    }
                    if (favorit)
                        ((PictureBox)sender).Image = Image.FromFile(filenameFavorit);
                    else
                        ((PictureBox)sender).Image = Image.FromFile(filenameFavoritFar);
                }
                ((PictureBox)sender).BorderStyle = BorderStyle.None;
            }
        }

        private void panel_MouseLeave(object sender, EventArgs e)
        {
            try
            {
                if (sender is Panel)
                {
                    ((Panel)sender).BackColor = colorDefault;
                    ((Panel)sender).BorderStyle = BorderStyle.None;
                    foreach (Object obj in ((Panel)sender).Controls)
                    {
                        if (obj is Label)
                        {
                            ((Label)obj).BackColor = colorDefault;
                        }
                        if (obj is RichTextBox)
                        {
                            ((RichTextBox)obj).BackColor = colorDefault;
                        }
                        if (obj is PictureBox)
                        {
                            if (currentVariant.Text == "Вариант 4")
                            {
                                bool favorit = false;
                                foreach (Object ob in ((Panel)sender).Controls)
                                {
                                    if (ob is Label)
                                    {
                                        if (((Label)ob).Visible == false)
                                            if (((Label)ob).Name == "favorit")
                                                if (((Label)ob).Text == "favorit")
                                                    favorit = true;
                                    }
                                }
                                if (favoritesShow)
                                {
                                    if (favorit)
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavorit);
                                    else
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFar);
                                }
                                if (favorit)
                                    ((PictureBox)obj).Image = Image.FromFile(filenameFavorit);    
                            }
                            else if (currentVariant.Text == "Вариант 5")
                            {
                                ((PictureBox)obj).Image = Image.FromFile(filenameDeleteFar);
                            }
                            else if (currentVariant.Text == "Вариант 6")
                            {
                                if (((Panel)sender).Parent.Name != "panelSearch")
                                {
                                    bool favorit = false;
                                    foreach (Object ob in ((Panel)sender).Controls)
                                    {
                                        if (ob is Label)
                                        {
                                            if (((Label)ob).Visible == false)
                                                if (((Label)ob).Name == "favorit")
                                                    if (((Label)ob).Text == "favorit")
                                                        favorit = true;
                                        }
                                    }
                                    if (favorit)
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavorit);
                                    else
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFar);
                                }
                                else
                                {
                                    bool favorit = false;
                                    foreach (Object ob in ((Panel)sender).Controls)
                                    {
                                        if (ob is Label)
                                        {
                                            if (((Label)ob).Visible == false)
                                                if (((Label)ob).Name == "favorit")
                                                    if (((Label)ob).Text == "favorit")
                                                        favorit = true;
                                        }
                                    }
                                    if (favorit)
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavorit);
                                    else
                                        ((PictureBox)obj).Image = Image.FromFile(filenameFavoritFar);
                                }

                            }
                        }
                        if (currentVariant.Text == "Вариант 4")
                        {//hideImage(((Panel)sender));
                        }
                        //if (currentVariant.Text == "Вариант 6")
                        //{
                        //    bool favorit = false;
                        //    foreach (Object ob in ((Panel)sender).Controls)
                        //    {
                        //        if (ob is Label)
                        //        {
                        //            if (((Label)ob).Visible == false && ((Label)ob).Name == "favorit")
                        //            {
                        //                if (((Label)ob).Text == "favorit")
                        //                {
                        //                    favorit = true;
                        //                }
                        //                else
                        //                    favorit = false;
                        //                foreach (Object o in ((Panel)sender).Controls)
                        //                    if (o is PictureBox)
                        //                        if (((Panel)sender).Parent.Name != "panelSearch")
                        //                            if (favorit)
                        //                                ((PictureBox)o).Image = Image.FromFile(filenameFavorit);
                        //                            else
                        //                                ((PictureBox)o).Image = Image.FromFile(filenameFavoritFar);
                        //            }
                        //        }
                        //    }
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                //MessageBox.Show("Непонятная ошибка\n"+ex.Message+"\n"+ex.StackTrace);
                logErorrs.Add("Непонятная ошибка\n" + ex.Message + "\n" + ex.StackTrace);
            }
        }

        //предыдущая метка, над которой был курсор
        string oldPanelLabel = "";
        //предыдущий уровень, над которым был курсор (обновляется каждый проход метода lbl_MouseHover)
        int oldPanelLevel = 0;
        private void labelDropDown_MouseHover(object sender, EventArgs e)
        {
            if (sender is Label)
            {

                ((Label)sender).BorderStyle = BorderStyle.None;
                ((Label)sender).Parent.BackColor = colorWhenMouseHover;
                foreach (Object obj in ((Label)sender).Parent.Controls)
                {
                    if (obj is Label)
                    {
                        ((Label)obj).BackColor = colorWhenMouseHover;
                        
                    }
                    
                }
                if (currentVariant.Text == "Вариант 6")
                {
                    bool favorit = false;
                    foreach (Object obj in ((Label)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            if (((Label)obj).Visible == false && ((Label)obj).Name == "favorit")
                            {
                                if (((Label)obj).Text == "favorit")
                                {
                                    favorit = true;
                                }
                                else
                                    favorit = false;
                                foreach (Object ob in ((Label)sender).Parent.Controls)
                                    if (ob is PictureBox)
                                        if (favorit)
                                            ((PictureBox)ob).Image = Image.FromFile(filenameDeleteSelect);
                                        else
                                            ((PictureBox)ob).Image = Image.FromFile(filenameFavoritSelect);
                            }
                        }
                    }

                }

            }
        }

        private void labelDropDown_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                ((Label)sender).Parent.BackColor = colorDefault;
                ((Label)sender).BorderStyle = BorderStyle.None;
                foreach (Object obj in ((Label)sender).Parent.Controls)
                {
                    if (obj is Label)
                    {
                        ((Label)obj).BackColor = colorDefault;
                    }
                }
                if (currentVariant.Text == "Вариант 6")
                {
                    bool favorit = false;
                    foreach (Object obj in ((Label)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            if (((Label)obj).Visible == false && ((Label)obj).Name == "favorit")
                            {
                                if (((Label)obj).Text == "favorit")
                                {
                                    favorit = true;
                                }
                                else
                                    favorit = false;
                                foreach (Object ob in ((Label)sender).Parent.Controls)
                                    if (ob is PictureBox)
                                        if (favorit)
                                            ((PictureBox)ob).Image = Image.FromFile(filenameDeleteFar);
                                        else
                                            ((PictureBox)ob).Image = Image.FromFile(filenameFavoritFar);
                            }
                        }
                    }

                }
            }
        }
        //возвращает метку с именем пункта меню по метке ">"
        Label getLabelNeighbor(Label l)
        {
            foreach (Object obj in l.Parent.Controls)
            {
                if (obj is Label)
                {
                    if (((Label)obj).Text != ">")
                        return ((Label)obj);
                }
            }
            return null;
        }
        //новые уровни менюхи подцеплять сюда
        private void labelDropDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Label)
            {
                if (((Label)sender).Text == ">" || ((Label)sender).Text == "")
                {
                    //определить уровень, на котором находимся
                    //закрыть все уровни ниже
                    switch (((Label)sender).Parent.Parent.Name)
                    {
                        case "myContextMenu":
                            {
                                panel1.Visible = false;
                                panel2.Visible = false;
                                panel3.Visible = false;
                                panel4.Visible = false;
                            }
                            break;
                        case "panel1":
                            {
                                panel2.Visible = false;
                                panel3.Visible = false;
                                panel4.Visible = false;
                            }
                            break;
                        case "panel2":
                            {
                                panel3.Visible = false;
                                panel4.Visible = false;
                            }
                            break;
                        case "panel3":
                            {
                                panel4.Visible = false;
                            }
                            break;
                        case "panel4": { }
                            break;
                    }
                }
                if (((Label)sender).Text != "")
                {
                    Label neighbor = getLabelNeighbor(((Label)sender));
                    curentPanel = (Panel)((Label)sender).Parent.Parent;
                    curentPanelLocation = new Point(curentPanel.Location.X, curentPanel.Location.Y);
                    currentClickItemX = ((Label)sender).Parent.Location.X;
                    currentClickItemY = ((Label)sender).Parent.Location.Y;
                    int panelWidth = ((Label)sender).Parent.Width;
                    for (int i = 0; i < listItems.Count; i++)
                    {
                        
                        if (neighbor.Text.IndexOf(listItems[i].name) != -1)
                        {
                            if (getChildren(listItems[i]).Count != 0)
                            {
                                addPanel(listItems[i], curentPanelLocation.X + currentClickItemX + panelWidth - 4, curentPanelLocation.Y + currentClickItemY, getChildren(listItems[i]), listItems[i].level);
                                if (curentPanel.Name != "myContextMenu")
                                {
                                    switch (curentPanel.Name)
                                    {
                                        case "panel1":
                                            {
                                                //уровень не поменялся, но пункт выбран другой
                                                if (oldPanelLevel == 1 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                {
                                                    //обновляем подчиненные панели
                                                    panel2.Visible = false;
                                                    panel3.Visible = false;
                                                    panel4.Visible = false;
                                                }

                                                oldPanelLevel = 1;

                                            }
                                            break;
                                        case "panel2":
                                            {
                                                //уровень не поменялся, но пункт выбран другой
                                                if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                {
                                                    //обновляем подчиненные панели
                                                    panel3.Visible = false;
                                                    panel4.Visible = false;
                                                }
                                                oldPanelLevel = 2;
                                            }
                                            break;
                                        case "panel3":
                                            {
                                                if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                {
                                                    //обновляем подчиненные панели
                                                    panel4.Visible = false;
                                                }
                                                oldPanelLevel = 3;
                                            }
                                            break;
                                        case "panel4":
                                            {
                                                if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                {
                                                    //обновляем подчиненные панели
                                                }
                                                oldPanelLevel = 4;
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    panel1.Visible = false;
                                    panel2.Visible = false;
                                    panel3.Visible = false;
                                    panel4.Visible = false;
                                    panelSearch.Visible = false;
                                    oldPanelLevel = 0;
                                }
                            }
                        }
                    }
                    oldPanelLabel = neighbor.Text;
                }
            }
        }

        private void lbl_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is Label)
            {
                ((Label)sender).BackColor = colorClick;
                ((Label)sender).BorderStyle = BorderStyle.None;
                ((Label)sender).Parent.BackColor = colorClick;
                foreach (Object obj in ((Label)sender).Parent.Controls)
                {
                    if (obj is Label)
                    {
                        ((Label)obj).BackColor = colorClick;
                        if (((Label)obj).Name == "dropDown" && ((Label)obj).Text == ">" || ((Label)obj).Name == "dropDown" && ((Label)obj).Text == "")
                        {
                            //определить уровень, на котором находимся
                            //закрыть все уровни ниже
                            switch (((Label)obj).Parent.Parent.Name)
                            {
                                case "myContextMenu":
                                    {
                                        panel1.Visible = false;
                                        panel2.Visible = false;
                                        panel3.Visible = false;
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel1":
                                    {
                                        panel2.Visible = false;
                                        panel3.Visible = false;
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel2":
                                    {
                                        panel3.Visible = false;
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel3":
                                    {
                                        panel4.Visible = false;
                                    }
                                    break;
                                case "panel4": { }
                                    break;
                            }
                        }
                        if (((Label)obj).Name == "dropDown" && ((Label)obj).Text == ">")
                        {
                            Label neighbor = getLabelNeighbor(((Label)sender));
                            curentPanel = (Panel)((Label)sender).Parent.Parent;
                            curentPanelLocation = new Point(curentPanel.Location.X, curentPanel.Location.Y);
                            currentClickItemX = ((Label)sender).Parent.Location.X;
                            currentClickItemY = ((Label)sender).Parent.Location.Y;
                            int panelWidth = ((Label)sender).Parent.Width;
                            for (int i = 0; i < listItems.Count; i++)
                            {
                        
                                if (neighbor.Text.IndexOf(listItems[i].name) != -1)
                                {
                                    if (getChildren(listItems[i]).Count != 0)
                                    {
                                        addPanel(listItems[i], curentPanelLocation.X + currentClickItemX + panelWidth - 4, curentPanelLocation.Y + currentClickItemY, getChildren(listItems[i]), listItems[i].level);
                                        if (curentPanel.Name != "myContextMenu")
                                        {
                                            switch (curentPanel.Name)
                                            {
                                                case "panel1":
                                                    {
                                                        //уровень не поменялся, но пункт выбран другой
                                                        if (oldPanelLevel == 1 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel2.Visible = false;
                                                            panel3.Visible = false;
                                                            panel4.Visible = false;
                                                            myContextMenu.Visible = true;
                                                        }

                                                        oldPanelLevel = 1;

                                                    }
                                                    break;
                                                case "panel2":
                                                    {
                                                        //уровень не поменялся, но пункт выбран другой
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel3.Visible = false;
                                                            panel4.Visible = false;
                                                        }
                                                        oldPanelLevel = 2;
                                                    }
                                                    break;
                                                case "panel3":
                                                    {
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel4.Visible = false;
                                                        }
                                                        oldPanelLevel = 3;
                                                    }
                                                    break;
                                                case "panel4":
                                                    {
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                        }
                                                        oldPanelLevel = 4;
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            panel1.Visible = false;
                                            panel2.Visible = false;
                                            panel3.Visible = false;
                                            panel4.Visible = false;
                                            panelSearch.Visible = false;
                                            oldPanelLevel = 0;
                                        }
                                    }
                                }
                            }
                            oldPanelLabel = neighbor.Text;
                        }
                        else if (((Label)obj).Name == "dropDown" && ((Label)obj).Text != ">")
                        {
                            foreach (Object ob in ((Label)sender).Parent.Controls)
                                if (ob is Label)
                                    if (((Label)ob).Name == "name")
                                    {
                                        richTextBoxLog.Text = sepLog + "\"" + ((Label)ob).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text;
                                        setLog("\"" + ((Label)ob).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text);
                                    }
                        }
                        //else if (((Label)obj).Name == "name")
                        //{
                        //    richTextBoxLog.Text = sepLog + "\"" + ((Label)obj).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text;
                        //    setLog("\"" + ((Label)obj).Text + "\"" + " выполнено.\n" + richTextBoxLog.Text);
                        //}
                    }
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                {
                    mouseRightButtonClick = true;
                }
                
            }
            else if (sender is RichTextBox)
            {
                ((RichTextBox)sender).BackColor = colorClick;
                ((RichTextBox)sender).Parent.BackColor = colorClick;
                string parent = "";
                string name = "";
                int level = 0;
                for (int i = 0; i < listItems.Count; i++)
                {
                    foreach (Object obj in ((RichTextBox)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            if (!((Label)obj).Visible)
                                if (((Label)obj).Name == "parent")
                                    parent = ((Label)obj).Text;
                                else if (((Label)obj).Name == "level")
                                    level = Convert.ToInt32(((Label)obj).Text);
                        }
                    }
                    
                    name = ((RichTextBox)sender).Text;
                    if (name.IndexOf("(") != -1)
                        name = name.Substring(0, name.IndexOf("("));
                    name = name.TrimEnd().TrimStart().ToLower();
                    if (name == listItems[i].name.TrimEnd().ToLower() && level == listItems[i].level && parent == listItems[i].parent)
                    {
                        if (!hasChildren(listItems[i]))
                        {
                            richTextBoxLog.Text = sepLog + "\"" + listItems[i].name + "\" выполнено.\n" + richTextBoxLog.Text;
                            setLog("\"" + name.TrimStart() + "\" выполнено.");
                            listItems[i].incClicks();
                            myContextMenu.Visible = false;
                            panelSearch.Visible = false;
                        }
                    }
                }
                if (e.Button == System.Windows.Forms.MouseButtons.Right)
                { 
                    mouseRightButtonClick = true; 
                }
                //((RichTextBox)sender).BorderStyle = BorderStyle.FixedSingle;
            }
            //richTextBoxLog.Text = "";
        }

        private void lbl_MouseUp(object sender, MouseEventArgs e)
        {
            if (sender is Label)
            {
                ((Label)sender).BackColor = colorWhenMouseHover;
                ((Label)sender).Parent.BackColor = colorWhenMouseHover;
                ((Label)sender).BorderStyle = BorderStyle.None;
                foreach (Object obj in ((Label)sender).Parent.Controls)
                {
                    if (obj is Label)
                        if (((Label)obj).Name == "dropDown")
                        {
                            ((Label)obj).BackColor = colorWhenMouseHover;
                        }
                }
            }
            else if (sender is RichTextBox)
            {
                ((RichTextBox)sender).BackColor = colorWhenMouseHover;
                ((RichTextBox)sender).Parent.BackColor = colorWhenMouseHover;
                //((RichTextBox)sender).BorderStyle = BorderStyle.FixedSingle;
            }
        }

        //назначает действие по нажатию на метку (пункт меню вариантов 3, 4, 5 и 6)
        private void labelItem_Click(object sender, EventArgs e)
        {
            if (sender is Label)
            {
                if (currentVariant.Text != "Вариант 5")
                {
                    setLog("Пункт меню \"" + ((Label)sender).Text + "\" выполнен.");
                    richTextBoxLog.Text = sepLog + "\"" + ((Label)sender).Text + "\" выполнено." + "\n" + richTextBoxLog.Text;
                }
                string parent = "";
                string name = "";
                int level = 0;
                foreach (Object obj in ((Label)sender).Parent.Controls)
                {
                    if (obj is Label)
                    {
                        if (((Label)obj).Visible == false)
                        {
                            if (((Label)obj).Name == "parent")
                                parent = getParent(((Label)obj));
                            else if (((Label)obj).Name == "level")
                                level = Convert.ToInt32(((Label)obj).Text);
                        }
                        else if (((Label)obj).Name == "name")
                        {
                            name = ((Label)obj).Text;
                        }
                    }
                }
                for (int i = 0; i < listItems.Count; i++)
                {
                    if (((Label)sender).Visible)
                        if (((Label)sender).Text.TrimStart().TrimEnd() == listItems[i].name.TrimStart().TrimEnd())
                        {
                            if (!hasChildren(listItems[i]) && parent == listItems[i].parent && level == listItems[i].level)
                            {
                                listItems[i].incClicks();
                                myContextMenu.Visible = false;
                                panelSearch.Visible = false;
                            }
                        }
                }
                foreach (Object obj in ((Label)sender).Parent.Controls)
                {
                    if (obj is Label)
                        if (((Label)obj).Text == "" && ((Label)obj).Name == "dropDown")
                        {
                            myContextMenu.Visible = false;
                            panelSearch.Visible = false;
                        }
                }
                
            }
            else if (sender is Panel)
            {
                setLog("Пункт меню \"" + ((Label)sender).Text + "\" выполнен.");
                richTextBoxLog.Text = sepLog + "\"" + ((Label)sender).Text + "\" выполнено." + "\n" + richTextBoxLog.Text;
                for (int i = 0; i < listItems.Count; i++)
                {
                    foreach (Object obj in ((Panel)sender).Controls)
                    {
                        if (((Label)obj).Text != ">" && ((Label)obj).Text != "")
                        {
                            if (obj is Label)
                            {
                                if (((Label)obj).Visible)
                                    if (((Label)obj).Text.IndexOf(listItems[i].name) != -1)
                                    {
                                        if (!hasChildren(listItems[i]))
                                        {
                                            listItems[i].incClicks();
                                            myContextMenu.Visible = false;
                                            panelSearch.Visible = false;
                                        }
                                    }
                            }
                        }
                    }
                }
                foreach (Object obj in ((Panel)sender).Controls)
                {
                    //открываешь dropDown
                    if (((Label)obj).Name == "dropDown")
                        if (((Label)obj).Text == ">")
                        {
                            Label neighbor = getLabelNeighbor(((Label)sender));
                            curentPanel = (Panel)((Label)sender).Parent.Parent;
                            curentPanelLocation = new Point(curentPanel.Location.X, curentPanel.Location.Y);
                            currentClickItemX = ((Label)sender).Parent.Location.X;
                            currentClickItemY = ((Label)sender).Parent.Location.Y;
                            int panelWidth = ((Label)sender).Parent.Width;
                            for (int i = 0; i < listItems.Count; i++)
                            {

                                if (neighbor.Text.IndexOf(listItems[i].name) != -1)
                                {
                                    if (getChildren(listItems[i]).Count != 0)
                                    {
                                        addPanel(listItems[i], curentPanelLocation.X + currentClickItemX + panelWidth - 4, curentPanelLocation.Y + currentClickItemY, getChildren(listItems[i]), listItems[i].level);
                                        if (curentPanel.Name != "myContextMenu")
                                        {
                                            switch (curentPanel.Name)
                                            {
                                                case "panel1":
                                                    {
                                                        //уровень не поменялся, но пункт выбран другой
                                                        if (oldPanelLevel == 1 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel2.Visible = false;
                                                            panel3.Visible = false;
                                                            panel4.Visible = false;
                                                        }

                                                        oldPanelLevel = 1;

                                                    }
                                                    break;
                                                case "panel2":
                                                    {
                                                        //уровень не поменялся, но пункт выбран другой
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel3.Visible = false;
                                                            panel4.Visible = false;
                                                        }
                                                        oldPanelLevel = 2;
                                                    }
                                                    break;
                                                case "panel3":
                                                    {
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                            panel4.Visible = false;
                                                        }
                                                        oldPanelLevel = 3;
                                                    }
                                                    break;
                                                case "panel4":
                                                    {
                                                        if (oldPanelLevel == 2 && oldPanelLabel.IndexOf(neighbor.Name) == -1)
                                                        {
                                                            //обновляем подчиненные панели
                                                        }
                                                        oldPanelLevel = 4;
                                                    }
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            panel1.Visible = false;
                                            panel2.Visible = false;
                                            panel3.Visible = false;
                                            panel4.Visible = false;
                                            panelSearch.Visible = false;
                                            oldPanelLevel = 0;
                                        }
                                    }
                                }
                            }
                            oldPanelLabel = neighbor.Text;
                        }
                        else
                        {
                            myContextMenu.Visible = false;
                            panelSearch.Visible = false;
                        }
                }
            }
            else if (sender is RichTextBox)
            {
                string name = "";
                string parent = "";
                int level = 0;
                
                for (int i = 0; i < listItems.Count; i++)
                {
                    foreach (Object obj in ((RichTextBox)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            if (!((Label)obj).Visible)
                                if (((Label)obj).Name == "parent")
                                    parent = ((Label)obj).Text;
                                else if (((Label)obj).Name == "level")
                                    level = Convert.ToInt32(((Label)obj).Text);
                        }
                    }
                    name = ((RichTextBox)sender).Text;
                    if (name.TrimStart() == listItems[i].name && level == listItems[i].level && parent == listItems[i].parent)
                    {
                        if (!hasChildren(listItems[i]))
                        {
                            richTextBoxLog.Text = sepLog + "\"" + name.TrimStart() + "\" выполнено.\n" + richTextBoxLog.Text;
                            setLog("\"" + name.TrimStart() + "\" выполнено.");
                            listItems[i].incClicks();
                            myContextMenu.Visible = false;
                            panelSearch.Visible = false;
                        }
                    }
                }
                myContextMenu.Visible = false;
                panelSearch.Visible = false;
            }
        }

        //определяет уровень пункта, считая пробелы перед ним
        int getLevelSpace(string s, string levelLenght)
        {
            int level = 0;
            int k = 0;
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                    k++;
                else
                    break;
                if (k == levelLenght.Length)
                { 
                    level++;
                    k = 0;
                }

            }
            return level;
        }
        //возвращает предка, ища скрытую метку с именем родителя на контрол-пункте меню
        string getParent(RichTextBox rtb)
        {
            foreach (Object obj in rtb.Parent.Controls)
            {
                if (obj is Label)
                {
                    if (!((Label)obj).Visible)
                        return ((Label)obj).Text;
                }
            }
            return "";
        }

        string getParent(Label label)
        {
            foreach (Object obj in label.Parent.Controls)
            {
                if (obj is Label)
                {
                    if (!((Label)obj).Visible)
                        return ((Label)obj).Text;
                }
            }
            return "";
        }

        private void pbDelete_Click(object sender, EventArgs e)
        {
            if (currentVariant.Text == "Вариант 3" || currentVariant.Text == "Вариант 5")
            {
                if (sender is PictureBox)
                {
                    string parent = "";
                    string name = "";
                    int level = 0;
                    foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            if (((Label)obj).Visible == false)
                            {
                                if (((Label)obj).Name == "parent")
                                    parent = getParent(((Label)obj));
                                else if (((Label)obj).Name == "level")
                                    level = Convert.ToInt32(((Label)obj).Text);
                            }
                            else if (((Label)obj).Name == "name")
                            {
                                name = ((Label)obj).Text;
                            }
                        }
                    }
                    for (int i = 0; i < listItems.Count; i++)
                    {
                        if (name.IndexOf(listItems[i].name) != -1 && parent == listItems[i].parent && level == listItems[i].level)
                        {
                            listItems[i].resetAllClicks();
                            richTextBoxLog.Text = sepLog + " Обнулили пункт меню \"" + listItems[i].name + "\"\n" + richTextBoxLog.Text;
                            clearMyPanel(panelSearch, 2);
                            if (currentVariant.Text == "Вариант 5")
                            {
                                clearMyPanel(panelSearch, 2);
                                getFastItemsForPanel();
                            }
                            else
                            {
                                clearMyPanel(panelSearch, 2);
                                getFastItemsForPanel();
                            }
                            

                                    
                        }
                    }
                }
            }
            else
            {
                if (sender is PictureBox)
                {
                    string str = "";
                    switch (currentVariant.Text)
                    {
                            case "Вариант 1":str = levelLenghtVar1;
                        break;
                            case "Вариант 2":str = levelLenghtVar2;
                        break;
                            case "Вариант 3":str = levelLenghtVar3;
                        break;
                            case "Вариант 4":str = levelLenghtVar4;
                        break;
                            case "Вариант 5":str = levelLenghtVar5;
                        break;
                            case "Вариант 6":str = levelLenghtVar6;
                        break;
                    }
                    string parent = "";
                    string name = "";
                    int level = 0;
                    foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                    {
                        if (obj is Label)
                        {
                            if (((Label)obj).Visible == false)
                            {
                                if (((Label)obj).Name == "parent")
                                    parent = getParent(((Label)obj));
                                else if (((Label)obj).Name == "level")
                                    level = Convert.ToInt32(((Label)obj).Text);
                            }
                            else if (((Label)obj).Name == "name")
                            {
                                name = ((Label)obj).Text;
                            }
                            
                        }
                    }

                    for (int i = 0; i < listItems.Count; i++)
                    {

                        if (name.IndexOf(listItems[i].name) != -1 && parent == listItems[i].parent && level == listItems[i].level)
                        {
                            listItems[i].favorit = false;
                            if (isIn(allFavorites, listItems[i]))
                                allFavorites.Remove(listItems[i]);
                            richTextBoxLog.Text = sepLog + " Убрали из избранного пункт меню \"" + listItems[i].name + "\"\n" + richTextBoxLog.Text;
                            
                            if (currentVariant.Text == "Вариант 6")
                            {
                                getFavoritesForPanel();
                                clearMyPanel(myContextMenu, 0);
                                if (panelTextBox.Text != "" && !favoritesShow)
                                {
                                    search(panelTextBox.Text);
                                }
                                else
                                {
                                    setMyContextMenu();
                                    setPanelSize(myContextMenu, defaultMyCMSSize.Width, defaultMyCMSSize.Height);
                                }

                            }
                            else
                            {
                                getFavoritesForPanel();
                                if (panelTextBox.Text != "" && !favoritesShow)
                                {
                                    clearMyPanel(panelSearch, 2);
                                    search(panelTextBox.Text);
                                }
                            }
                        }
                    }
                }
            }
        }

        private void pbFavorit_Click(object sender, EventArgs e)
        {

            string name = "";
            string parent = "";
            int level = 0;
            string favorit = "";
            string fullname = "";
            if (sender is PictureBox)
            {
                foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                {
                    if (obj is Label)
                    {
                        //поймали скрытую метку
                        if (((Label)obj).Visible == false)
                        {
                            if (((Label)obj).Name == "parent")
                                parent = ((Label)obj).Text;
                            if (((Label)obj).Name == "favorit")
                                favorit = ((Label)obj).Text;
                            if (((Label)obj).Name == "level")
                                level = Convert.ToInt32(((Label)obj).Text);
                            if (((Label)obj).Name == "toolTipName")
                                fullname = ((Label)obj).Text;
                        }
                        else if (((Label)obj).Name == "name")
                            name = ((Label)obj).Text;
                    }
                    if (obj is RichTextBox)
                    {
                        name = ((RichTextBox)obj).Text;
                    }
                    char[] c = { ' ' };
                    if (name.IndexOf("(") != -1)
                        name = name.Substring(0, name.IndexOf("("));
                    name = name.TrimEnd().TrimStart().ToLower();
                    //name = myTrimStart(name);
                }
                for (int i = 0; i < listItems.Count; i++)
                {
                    string n = listItems[i].name.ToLower().TrimEnd();
                    if (name == n && parent == listItems[i].parent && level == listItems[i].level ||
                        fullname.TrimEnd().TrimStart().ToLower() == n && parent == listItems[i].parent && level == listItems[i].level)
                    {
                        if (favorit != "favorit")
                        {
                            if (!hasChildren(listItems[i]))
                            {
                                listItems[i].favorit = true;
                                if (!isIn(allFavorites, listItems[i]))
                                {
                                    allFavorites.Add(listItems[i]);
                                    richTextBoxLog.Text = sepLog + " Добавили в избранное пункт меню \"" + listItems[i].name + "\"\n" + richTextBoxLog.Text;
                                    if (currentVariant.Text == "Вариант 4")
                                    {
                                        clearMyPanel(panelSearch, 2);
                                        search(panelTextBox.Text.ToLower());
                                    }
                                    else
                                    {
                                        string request = panelTextBox.Text;
                                        getFavoritesForPanel();
                                        clearMyPanel(myContextMenu, 0);
                                        if (request != "")
                                        {
                                            search(request.ToLower());
                                        }
                                        else
                                        {
                                            setMyContextMenu();
                                            setPanelSize(myContextMenu, defaultMyCMSSize.Width, defaultMyCMSSize.Height);
                                        }
                                        panelTextBox.Text = request;
                                    }
                                }
                                else
                                {
                                    richTextBoxLog.Text = sepLog + " Пункт меню \"" + listItems[i].name + "\" уже находится в избранном.\n" + richTextBoxLog.Text;
                                }

                                foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                                {
                                    ((PictureBox)sender).Image = Image.FromFile(filenameFavoritSelect);
                                    //((PictureBox)sender).MouseHover += new EventHandler(pb_MouseHover);
                                    //((PictureBox)sender).MouseLeave += new EventHandler(pb_MouseLeave);
                                    //((PictureBox)sender).Click += new EventHandler(pbDelete_Click);
                                    //((PictureBox)sender).Parent.MouseHover += new EventHandler(panel_MouseHover);
                                    //((PictureBox)sender).Parent.MouseLeave += new EventHandler(panel_MouseLeave);
                                }
                                //getFavoritesForPanel();
                                //clearMyPanel(myContextMenu, 0);
                                //setMyContextMenu();
                            }
                        }
                        else
                        {
                            listItems[i].favorit = false;
                            if (isIn(allFavorites, listItems[i]))
                                allFavorites.Remove(listItems[i]);
                            richTextBoxLog.Text = sepLog + " Убрали из избранного пункт меню \"" + listItems[i].name + "\"\n" + richTextBoxLog.Text;
                            if (currentVariant.Text == "Вариант 6")
                            {
                                string request = panelTextBox.Text;
                                getFavoritesForPanel();
                                clearMyPanel(myContextMenu, 0);
                                if (request != "")
                                    search(request.ToLower());
                                else
                                {
                                    setMyContextMenu();
                                    setPanelSize(myContextMenu, defaultMyCMSSize.Width, defaultMyCMSSize.Height);
                                }
                                panelTextBox.Text = request;
                            }
                            else
                            {
                                clearMyPanel(panelSearch, 2);
                                search(panelTextBox.Text.ToLower());
                            }
                            foreach (Object obj in ((PictureBox)sender).Parent.Controls)
                            {
                                ((PictureBox)sender).Image = Image.FromFile(filenameFavoritFarSelect);
                                //((PictureBox)sender).MouseHover += new EventHandler(pb_MouseHover);
                                //((PictureBox)sender).MouseLeave += new EventHandler(pb_MouseLeave);
                                //((PictureBox)sender).Click += new EventHandler(pbFavorit_Click);
                                //((PictureBox)sender).Parent.MouseHover += new EventHandler(panel_MouseHover);
                                //((PictureBox)sender).Parent.MouseLeave += new EventHandler(panel_MouseLeave);
                            }
                            //getFavoritesForPanel();
                            //clearMyPanel(myContextMenu, 0);
                            //setMyContextMenu();
                        }
                    }
                }
            }

        }

        //удаляет все начальные пробелы
        string myTrimStart(string s)
        {
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == ' ')
                    s = s.Substring(i + 1);
                else
                    return s;
            }
            return "";
        }

        //проверяет наличие указанного элемента в списке
        bool isIn(List<Item> l, Item i)
        {
            foreach (Item item in l)
            {
                if (item.name == i.name && item.parent == i.parent && item.level == i.level)
                    return true;
            }
            return false;
        }

        private void panel1_VisibleChanged(object sender, EventArgs e)
        {
            if (((Panel)sender).Visible)
                panelSearch.Visible = false;
            else
            {
                panelSearch.Visible = true;
                switch (((Panel)sender).Name)
                {
                    case "panel1":
                        {
                            maxWidthCMSlevel1 = 0;
                            maxHeightCMSlevel1 = 0;
                        }
                        break;
                    case "panel2":
                        {
                            maxWidthCMSlevel2 = 0;
                            maxHeightCMSlevel2 = 0;
                        }
                        break;
                    case "panel3":
                        {
                            maxWidthCMSlevel3 = 0;
                            maxHeightCMSlevel3 = 0;
                        }
                        break;
                    case "panel4":
                        {
                            maxWidthCMSlevel4 = 0;
                            maxHeightCMSlevel4 = 0;
                        }
                        break;
                }
                clearMyPanel(((Panel)sender), 0);
            }
        }

        private void myContextMenu_VisibleChanged(object sender, EventArgs e)
        {
            if (((Panel)sender).Visible)
            {
                if (currentVariant.Text == "Вариант 3" || currentVariant.Text == "Вариант 4")
                    if (panelSearch.Visible)
                        ((Panel)sender).Visible = false;

                myContextMenu.Size = new Size(defaultWidthCMS, defaultHeightCMS);
            }
            else
            {
                panel1.Visible = false;
                panel2.Visible = false;
                panel3.Visible = false;
                panel4.Visible = false;
            }
            labelToolTip.Visible = false;
        }

        private void labelClose_Click(object sender, EventArgs e)
        {
            panelSearch.Visible = false;
        }

        private void myContextMenu_LocationChanged(object sender, EventArgs e)
        {
            panel1.Visible = false;
            panel2.Visible = false;
            panel3.Visible = false;
            panel4.Visible = false;
        }

        private void textBoxFontSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                //запрет на ввод более одной десятичной точки и тире
                if (
                    (e.KeyChar != ',' || ((ToolStripTextBox)sender).Text.IndexOf(",") != -1)
                   )
                {
                    e.Handled = true;
                }

            }
        }

        private void textBoxFastResult_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!Char.IsDigit(e.KeyChar) && e.KeyChar != '\b')
            {
                
                e.Handled = true;

            }
        }

        private void lbl_VisibleChanged(object sender, EventArgs e)
        {
            lbl.Visible = false;
        }

        private void comboBoxFont_TextChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripControlHost)
            {
                if (((ToolStripControlHost)sender).Text != "")
                {
                    clearMyPanel(myContextMenu, 0);
                    search(textBoxSearch.Text);
                }
            }
            else if (sender is ComboBox)
            {
                if (((ComboBox)sender).Text != "")
                {
                    clearMyPanel(myContextMenu, 0);
                    search(textBoxSearch.Text);
                }
            }

        }

        private void цветаВыделенийToolStripMenuItem_DropDownOpened(object sender, EventArgs e)
        {
            if (!colorsWasDownloaded)
            {
                setAllColor();
                colorsWasDownloaded = true;
                Text = Text.Substring("Загружаются списки цветов... ".Length, Text.Length - "Загружаются списки цветов... ".Length);
            }
        }

        private void цветаВыделенийToolStripMenuItem_MouseHover(object sender, EventArgs e)
        {
            if (!colorsWasDownloaded)
                Text = "Загружаются списки цветов... " + Text;
            
        }

        void ColorOnClick(object sender, EventArgs e)
        {
            switch (((ToolStripMenuItem)sender).OwnerItem.Name)
            {
                case "toolsBackcolorDefault": colorDefault = setColor(((ToolStripMenuItem)sender).Text);
                    break;
                case "toolsMouseHover": colorWhenMouseHover = setColor(((ToolStripMenuItem)sender).Text);
                    break;
                case "toolsBorder": colorBorder = setColor(((ToolStripMenuItem)sender).Text);
                    break;
                case "toolsMouseClick": colorClick = setColor(((ToolStripMenuItem)sender).Text);
                    break;
                case "toolsTextDefault": colorText = setColor(((ToolStripMenuItem)sender).Text);
                    break;
                case "toolsTextNotEnabled": colorNotEnabled = setColor(((ToolStripMenuItem)sender).Text);
                    break;
            }
            clearMyPanel(myContextMenu, 0);
            search(textBoxSearch.Text);
        }

        Color setColor(string s)
        {
            string ss = "";
            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] != ' ')
                    ss += s;
            }
            foreach (PropertyInfo pi in api)
            {
                if (pi.CanRead && pi.PropertyType == typeof(Color)
                    && pi.Name != "Transparent")
                {
                    // Извлекаем очередной цвет
                    Color color = (Color)pi.GetValue(null, null);
                    string colosS = color.Name;
                    if (ss == colosS)
                    {
                        return color;
                    }
                }

            }
            return Color.WhiteSmoke;
        }

        void saveAllTools()
        {

            string filename = Application.StartupPath + "\\#tools.txt";
            StreamWriter sw = new StreamWriter(filename);
            sw.WriteLine("colorDefault:"+colorDefault.Name);
            sw.WriteLine("colorWhenMouseHover:" + colorWhenMouseHover.Name);
            sw.WriteLine("colorBorder:" + colorBorder.Name);
            sw.WriteLine("colorClick:" + colorClick.Name);
            sw.WriteLine("colotText:" + colorText.Name);
            sw.WriteLine("colorNotEnabled:" + colorNotEnabled.Name);
            sw.Close();
        }

        

        void setAllTools()
        {
            string filename = Application.StartupPath + "\\#tools.txt";
            StreamReader sr = new StreamReader(filename);
            string sBig = sr.ReadToEnd();
            sr.Close();
            string s = "";
            for (int i = 0; i < sBig.Length; i++)
            {
                switch (s)
                {
                    case "colorDefault:":
                        {
                            string tool = "";
                            while (sBig[i] != '\n')
                            {
                                if (i >= sBig.Length)
                                    break;
                                if (sBig[i] != '\r')
                                    tool += sBig[i];
                                i++;
                            }
                            colorDefault = setColor(tool);
                            s = "";
                        }
                        break;
                    case "colorWhenMouseHover:":
                        {
                            string tool = "";
                            while (sBig[i] != '\n' || i >= sBig.Length)
                            {
                                if (i >= sBig.Length)
                                    break;
                                if (sBig[i] != '\r')
                                    tool += sBig[i];
                                i++;
                            }
                            colorWhenMouseHover = setColor(tool);
                            s = "";
                        }
                        break;
                    case "colorBorder:":
                        {
                            string tool = "";
                            while (sBig[i] != '\n' || i >= sBig.Length)
                            {
                                if (i >= sBig.Length)
                                    break;
                                if (sBig[i] != '\r')
                                    tool += sBig[i];
                                i++;
                            }
                            colorBorder = setColor(tool);
                            s = "";
                        }
                        break;
                    case "colorClick:":
                        {
                            string tool = "";
                            while (sBig[i] != '\n' || i >= sBig.Length)
                            {
                                if (i >= sBig.Length)
                                    break;
                                if (sBig[i] != '\r')
                                    tool += sBig[i];
                                i++;
                            }
                            colorClick = setColor(tool);
                            s = "";
                        }
                        break;
                    case "colotText:":
                        {
                            string tool = "";
                            while (sBig[i] != '\n' || i >= sBig.Length)
                            {
                                if (i >= sBig.Length)
                                    break;
                                if (sBig[i] != '\r')
                                    tool += sBig[i];
                                i++;
                            }
                            colorText = setColor(tool);
                            s = "";
                        }
                        break;
                    case "colorNotEnabled:":
                        {
                            string tool = "";
                            while (sBig[i] != '\n' || i >= sBig.Length)
                            {
                                if (i >= sBig.Length)
                                    break;
                                if (sBig[i] != '\r')
                                    tool += sBig[i];
                                i++;
                            }
                            colorNotEnabled = setColor(tool);
                            s = "";
                        }
                        break;
                        
                }
                s += sBig[i];
            }
        }

        private void окноПеременныхToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string s = "";
            if (currentVariant.Text == "Вариант 5")
            { 
                List<string> l = getAllKolClicks();
                for (int i = 0; i < l.Count; i++)
                    s += l[i] + "\n";
                MessageBox.Show(s);
            }
            else if (currentVariant.Text == "Вариант 6")
            {
                List<Item> l = getAllFavorites();
                s += "Список избранных пунктов: \n";
                for (int i = 0; i < l.Count; i++)

                    s += l[i].name + "(level:" + l[i].level + "; parent:" + l[i].parent + "); \n"; 
                MessageBox.Show(s);
            }
            
        }

        void a()
        {
            string s = "";
            List<Item> l = getAllFavorites();
            s += "Список избранных пунктов: \n";
            for (int i = 0; i < l.Count; i++)

                s += l[i].name + /*"(level:" + l[i].level + "; parent:" + l[i].parent + ");*/"; \n";
            //MessageBox.Show(s);
            richTextBoxLog.Text = s + "\n" + richTextBoxLog.Text;
        }

        private void changeVariant(object sender, EventArgs e)
        {
            if (sender is Button)
            {
                switch (((Button)sender).Name)
                {
                    case "buttonVar1":
                        {
                            currentVariant.Text = "Вариант 1";
                            textBoxFileName.Text = "Сейчас используется: " + ((Button)sender).Text + " ; " + myShortFileName + ".";
                        }
                        break;
                    case "buttonVar2":
                        {
                            currentVariant.Text = "Вариант 2";
                            textBoxFileName.Text = "Сейчас используется: " + ((Button)sender).Text + " ; " + myShortFileName + ".";
                        }
                        break;
                    case "buttonVar3":
                        {
                            currentVariant.Text = "Вариант 3";
                            textBoxFileName.Text = "Сейчас используется: " + ((Button)sender).Text + " ; " + myShortFileName + ".";
                            
                        }
                        break;
                    case "buttonVar4":
                        {
                            currentVariant.Text = "Вариант 4";
                            textBoxFileName.Text = "Сейчас используется: " + ((Button)sender).Text + " ; " + myShortFileName + ".";
                            
                        }
                        break;
                    case "buttonVar5":
                        {
                            currentVariant.Text = "Вариант 5";
                            textBoxFileName.Text = "Сейчас используется: " + ((Button)sender).Text + " ; " + myShortFileName + ".";
                        }
                        break;
                    case "buttonVar6":
                        {
                            currentVariant.Text = "Вариант 6";
                            textBoxFileName.Text = "Сейчас используется: " + ((Button)sender).Text + " ; " + myShortFileName + ".";
                        }
                        break;
                }
            }
        }

        private void item_DropDownItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            if (e.ClickedItem.Text != "" && e.ClickedItem.Text != "-" && ((ToolStripMenuItem)e.ClickedItem).DropDownItems.Count == 0)
            {
                richTextBoxLog.Text = sepLog + "\"" + e.ClickedItem.Text.TrimStart() + "\" выполнено.\n" + richTextBoxLog.Text;
                setLog("\"" + e.ClickedItem.Text.TrimStart() + "\" выполнено.\n" + richTextBoxLog.Text);
            }
        }

        private void labelItem1_MouseLeave(object sender, EventArgs e)
        {
            labelToolTip.Visible = false;
            labelToolTip.Text = "";
        }

        private void textBoxTopLeftAnglePanel_TextChanged(object sender, EventArgs e)
        {
            if (((ToolStripTextBox)sender).Name == "textBoxTopLeftAnglePanel")
                topLeftAnglePanel = getPointFromStr(((ToolStripTextBox)sender).Text);
            else if (((ToolStripTextBox)sender).Name == "textBoxTopLeftAngleLabel")
                topLeftAngleLabel = getPointFromStr(((ToolStripTextBox)sender).Text);

        }

        Point getPointFromStr(string s)
        {
            try { 
                Point p = new Point();
                bool wasComma = false;
                string top = "";
                string left = "";
                for (int i = 0; i < s.Length; i++)
                {
                    if (!wasComma)
                    {
                        if (s[i] != ';')
                        {
                            left += s[i];
                        }
                        else
                            wasComma = true;
                    }
                    else
                    {
                        top += s[i];
                    }
                }
                p = new Point(Convert.ToInt32(left), Convert.ToInt32(top));
                return p;
            
            }
            catch {
                MessageBox.Show("Локацию метки или панели следует задавать следующим образом: left; top", "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return new Point(0,0);
            }
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            keysReactMyContextMenu(e);
        }

        
        private void listBoxDynamic_KeyDown(object sender, KeyEventArgs e)
        {
            keysReactMyContextMenu(e);
        }

        int idxFocusetItemDown = 0;
        int idxFocusetItemDown1 = 0;
        int idxFocusetItemDown2 = 0;
        int idxFocusetItemDown3 = 0;
        int idxFocusetItemDown4 = 0;
        //реакция на клавиши моих менюшек
        void keysReactMyContextMenu(KeyEventArgs e)
        {
            try
            {
                //нажата клавиша "контекстное меню"
                if (e.KeyCode == Keys.Apps)
                {


                    //MessageBox.Show(AppsClick.ToString());
                    switch (currentVariant.Text)
                    {
                        case "Вариант 1":
                            {
                                textBoxSearch.TextBox.ContextMenu = null;
                                textBoxSearch.TextBox.ContextMenuStrip = contextMenuStripTextBox;
                            }
                            break;
                        case "Вариант 2":
                            {
                                textBoxSearch.TextBox.ContextMenu = null;
                                textBoxSearch.TextBox.ContextMenuStrip = contextMenuStripTextBox;
                            }
                            break;
                        case "Вариант 3": { panelSearch.Visible = true; }
                            break;
                        case "Вариант 4": { panelSearch.Visible = true; }
                            break;
                        case "Вариант 5":
                            {
                                myContextMenu.Visible = true;
                                panelSearch.Visible = true;
                            }
                            break;
                        case "Вариант 6":
                            {
                                myContextMenu.Visible = true;
                                panelSearch.Visible = true;
                            }
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Escape)
                {
                    switch (currentVariant.Text)
                    {
                        case "Вариант 1": { textBoxSearch.Focus(); }
                            break;
                        case "Вариант 2": { textBoxSearch.Focus(); }
                            break;
                        case "Вариант 3": { panelSearch.Visible = false; }
                            break;
                        case "Вариант 4": { panelSearch.Visible = false; }
                            break;
                        case "Вариант 5":
                            {
                                myContextMenu.Visible = false;
                                panelSearch.Visible = false;
                            }
                            break;
                        case "Вариант 6":
                            {
                                myContextMenu.Visible = false;
                                panelSearch.Visible = false;
                            }
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Left)
                {
                    switch (currentVariant.Text)
                    {
                        case "Вариант 1": { textBoxSearch.Focus(); }
                            break;
                        case "Вариант 2": { textBoxSearch.Focus(); }
                            break;
                        case "Вариант 3":
                            {
                                foreach (Object obj in panelSearch.Controls)
                                {
                                    if (obj is Panel)
                                    {
                                        ((Panel)obj).Focus();
                                        ((Panel)obj).BackColor = colorWhenMouseHover;
                                    }
                                }
                            }
                            break;
                        case "Вариант 4":
                            {

                                foreach (Object obj in panelSearch.Controls)
                                {

                                    if (obj is Panel)
                                    {
                                        ((Panel)obj).Focus();
                                        ((Panel)obj).BackColor = colorWhenMouseHover;
                                    }
                                }
                            }
                            break;
                        case "Вариант 5":
                            {
                                foreach (Object obj in panelSearch.Controls)
                                {
                                    if (obj is Panel)
                                    {
                                        ((Panel)obj).Focus();
                                        ((Panel)obj).BackColor = colorWhenMouseHover;

                                    }
                                }
                            }
                            break;
                        case "Вариант 6":
                            {
                                foreach (Object obj in panelSearch.Controls)
                                {
                                    if (obj is Panel)
                                    {
                                        ((Panel)obj).Focus();
                                        ((Panel)obj).BackColor = colorWhenMouseHover;
                                    }
                                }
                            }
                            break;
                    }
                }
                if (contextMenu.Visible)
                if (e.KeyCode == Keys.Down)
                {
                    if (idxFocusetItemDown < contextMenu.Items.Count - 1)
                    {
                        idxFocusetItemDown++;
                    }
                    switch (currentVariant.Text)
                    {
                        case "Вариант 1": {
                            if (contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripSeparator" && contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripTextBox")
                                contextMenu.Items[idxFocusetItemDown].Select(); 
                            else
                                if (idxFocusetItemDown + 1 <= contextMenu.Items.Count - 1)
                                {
                                    idxFocusetItemDown++;
                                    contextMenu.Items[idxFocusetItemDown].Select(); 
                                }
                        }
                            break;
                        case "Вариант 2":
                            {
                                if (contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripSeparator" && contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripTextBox")
                                    contextMenu.Items[idxFocusetItemDown].Select();
                                else
                                    if (idxFocusetItemDown + 1 <= contextMenu.Items.Count - 1)
                                    {
                                        idxFocusetItemDown++;
                                        contextMenu.Items[idxFocusetItemDown].Select();
                                    }
                            }
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Up)
                {
                    if (idxFocusetItemDown > 0)
                    { 
                        idxFocusetItemDown--;
                    }
                    
                    switch (currentVariant.Text)
                    {
                        case "Вариант 1":
                            {
                                if (contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripSeparator" && contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripTextBox")
                                    contextMenu.Items[idxFocusetItemDown].Select();
                                else
                                    if (idxFocusetItemDown - 1 >= 0 )
                                    {
                                        idxFocusetItemDown--;
                                        contextMenu.Items[idxFocusetItemDown].Select();
                                    }
                            }
                            break;
                        case "Вариант 2":
                            {
                                if (contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripSeparator" && contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripTextBox")
                                    contextMenu.Items[idxFocusetItemDown].Select();
                                else
                                    if (idxFocusetItemDown - 1 >= 0)
                                    {
                                        idxFocusetItemDown--;
                                        contextMenu.Items[idxFocusetItemDown].Select();
                                    }
                            }
                            break;
                    }
                }
                else if (e.KeyCode == Keys.Right)
                {
                    if (contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripSeparator" && contextMenu.Items[idxFocusetItemDown].GetType().ToString() != "System.Windows.Forms.ToolStripTextBox")
                        if (((ToolStripMenuItem)contextMenu.Items[idxFocusetItemDown]).GetType().ToString() != "System.Windows.Forms.ToolStripSeparator" && ((ToolStripMenuItem)contextMenu.Items[idxFocusetItemDown]).GetType().ToString()  != "System.Windows.Forms.ToolStripTextBox")
                            if (((ToolStripMenuItem)contextMenu.Items[idxFocusetItemDown]).DropDownItems.Count != 0)
                                if (idxFocusetItemDown1 < ((ToolStripMenuItem)contextMenu.Items[idxFocusetItemDown]).DropDownItems.Count)
                                {
                                    if (((ToolStripMenuItem)contextMenu.Items[idxFocusetItemDown]).DropDownItems.Count != 0)
                                    {
                                        ((ToolStripMenuItem)contextMenu.Items[idxFocusetItemDown]).ShowDropDown();
                                        ((ToolStripMenuItem)contextMenu.Items[idxFocusetItemDown]).DropDownItems[idxFocusetItemDown1].Select();
                                    }
                                }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\n" + ex.StackTrace);
                logErorrs.Add(ex.Message + "\n" + ex.StackTrace);
            }
        }

        private void panelTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            keysReactMyContextMenu(e);
        }

        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            textBoxSearch.TextBox.ContextMenu = null;
            textBoxSearch.TextBox.ContextMenuStrip = contextMenuStripTextBox;
            keysReactMyContextMenu(e);
        }

        private void developer_Click(object sender, EventArgs e)
        {
            Developer d = new Developer();
            d.ShowDialog();
        }

        private void item_MouseEnter(object sender, EventArgs e)
        {

            if (sender is ToolStripMenuItem)
            {
                ((ToolStripMenuItem)sender).Select();
            }
            else if (sender is ToolStripItem)
            {
                ((ToolStripItem)sender).Select();
            }
        }

        private void panel_Enter(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                int rtbWidth = 0;
                int pWidth = 0;
                foreach (Object ob in ((Panel)sender).Controls)
                {
                    if (ob is Label)
                    {
                        if (((Label)ob).Visible == false)
                        {
                            if (((Label)ob).Name == "maxKolWords")
                                pWidth = Convert.ToInt32(((Label)ob).Text);
                            else if (((Label)ob).Name == "kolWords")
                                rtbWidth = Convert.ToInt32(((Label)ob).Text);
                        }
                    }
                }
                //((Panel)sender).BorderStyle = BorderStyle.FixedSingle;
                foreach (Object obj in ((Panel)sender).Controls)
                {
                    if (obj is RichTextBox)
                    {
                        if (currentVariant.Text == "Вариант 3")
                        {
                            foreach (Object ob in ((RichTextBox)obj).Parent.Controls)
                            {
                                if (ob is Label)
                                {
                                    if (((Label)ob).Visible == false)
                                        if (rtbWidth > maxKolChar)
                                            if (((Label)ob).Name == "toolTipName")
                                            {
                                                int x = ((RichTextBox)obj).Parent.Parent.Location.X + ((RichTextBox)obj).Parent.Width;
                                                int y = ((RichTextBox)obj).Parent.Parent.Location.Y + ((RichTextBox)obj).Parent.Location.Y + (int)((RichTextBox)obj).Parent.Height / 3;
                                                viewToolTip(((Label)ob).Text, x, y, true);
                                            }

                                }
                            }
                        }
                        else if (currentVariant.Text == "Вариант 4")
                        {
                            //viewImage((Panel)((RichTextBox)sender).Parent);
                            bool favorit = false;
                            foreach (Object ob in ((RichTextBox)obj).Parent.Controls)
                            {
                                if (ob is Label)
                                {
                                    if (((Label)ob).Visible == false)
                                        if (rtbWidth > maxKolChar)
                                            if (((Label)ob).Name == "toolTipName")
                                            {
                                                int x = ((RichTextBox)obj).Parent.Parent.Location.X + ((RichTextBox)obj).Parent.Width;
                                                int y = ((RichTextBox)obj).Parent.Parent.Location.Y + ((RichTextBox)obj).Parent.Location.Y + (int)((RichTextBox)obj).Parent.Height / 3;
                                                viewToolTip(((Label)ob).Text, x, y, true);
                                            }

                                }
                            }
                        }
                        else if (currentVariant.Text == "Вариант 5")
                        {
                            foreach (Object ob in ((RichTextBox)obj).Parent.Controls)
                            {
                                if (ob is Label)
                                {
                                    if (((Label)ob).Visible == false)
                                        if (rtbWidth > maxKolChar)
                                            if (((Label)ob).Name == "toolTipName")
                                            {
                                                labelToolTip.Text = ((Label)ob).Text;
                                                int x = ((RichTextBox)obj).Parent.Parent.Location.X - labelToolTip.Width - 7;
                                                int y = ((RichTextBox)obj).Parent.Parent.Location.Y + ((RichTextBox)obj).Parent.Location.Y + (int)((RichTextBox)obj).Parent.Height / 3;
                                                viewToolTip(((Label)ob).Text, x, y, false);
                                            }
                                }
                            }
                        }
                        else if (currentVariant.Text == "Вариант 6")
                        {
                            foreach (Object ob in ((RichTextBox)obj).Parent.Controls)
                            {
                                if (ob is Label)
                                {
                                    if (((Label)ob).Visible == false)
                                        if (rtbWidth > maxKolChar)
                                            if (((Label)ob).Name == "toolTipName")
                                            {
                                                labelToolTip.Text = ((Label)ob).Text;
                                                int x = ((RichTextBox)obj).Parent.Parent.Location.X - labelToolTip.Width - 7;
                                                int y = ((RichTextBox)obj).Parent.Parent.Location.Y + ((RichTextBox)obj).Parent.Location.Y + (int)((RichTextBox)obj).Parent.Height / 3;
                                                viewToolTip(((Label)ob).Text, x, y, false);
                                            }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void myContextMenu_Resize(object sender, EventArgs e)
        {
            panelSearch.Location = new Point(((Panel)sender).Location.X + ((Panel)sender).Width, ((Panel)sender).Location.Y);
        }

        private void Form1_Click(object sender, EventArgs e)
        {
            //closeAll();
        }

        private void panelSearch_Leave(object sender, EventArgs e)
        {
            switch (currentVariant.Text)
            {
                case "Вариант 1": { closeAll(); }
                    break;
                case "Вариант 2": { closeAll(); }
                    break;
                case "Вариант 3": { closeAll(); }
                    break;
                case "Вариант 4": { closeAll(); }
                    break;
                case "Вариант 5":
                    {
                        if (panelSearch.Visible != false && myContextMenu.Visible)
                        { 
                            closeAll();
                        }
                    }
                    break;
                case "Вариант 6":
                    {
                        if (panelSearch.Visible != false && myContextMenu.Visible)
                        {
                            closeAll();
                        }
                    }
                    break;
            }
        }

        void closeAll()
        {
            panelSearch.Visible = false;
            myContextMenu.Visible = false;
        }

        private void groupBoxDynamic_Enter(object sender, EventArgs e)
        {
            //closeAll();
        }

        private void richTextBoxLog_Enter(object sender, EventArgs e)
        {

        }

        private void pnl_Enter(object sender, EventArgs e)
        {
            if (sender is Panel)
            {
                if (((Panel)sender).Focused)
                {
                    ((Panel)sender).BackColor = colorWhenMouseHover;

                }
            }
        }
    }
}
