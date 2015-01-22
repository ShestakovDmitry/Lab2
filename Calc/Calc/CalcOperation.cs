using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Calc
{
    public class CalcOperation : WebControl
    {
        //Контролы
        private DropDownList DDLOperation;
        private DropDownList DDLFunction;
        private TextBox TBox;

        //Значение которое ввели в TextBox 
        private double TBoxValue;
        //Номер контрола для генерации ID
        private int CNum;
        //Значение передаваемое на вход контрола
        public double IValue
        {
            get
            {
                object o = ViewState["IValue"];
                return (o == null) ? 0 : (double)o;
            }

            set
            {
                ViewState["IValue"] = value;
            }
        }
        //Значение получаемое на выходе контрола
        private double OValue
        {
            get
            {
                object o = ViewState["OValue"];
                return (o == null) ? 0 : (double)o;
            }

            set
            {
                ViewState["OValue"] = value;
            }
        }
        //Метод для получения выхода контрола
        public double GetOValue()
        {
            return OValue;
        }
        //Строка ошибки
        public string ErrorStr { get { return ErrorString; } }
        private string ErrorString;
        //Метод возвращающий результат выбранной операции
        protected double GetOperationResult(double TBValue)
        { 
            switch ((string)DDLOperation.SelectedValue)
            {
                case "+": return (IValue + TBValue);
                case "-": return (IValue - TBValue);
                case "*": return (IValue * TBValue);
                case "/": 
                        {   if (TBValue != 0)
                            { return (IValue / TBValue); }
                        else
                        {
                            ErrorString = "Деление на 0.";
                            return 0; }
                    };
                case "^": return (Math.Pow(IValue, TBValue));
                default: return 0;
            }
        }
        //Метод возвращающий результат выбранной функции
        protected double GetFunctionResult()
        {
            switch ((string)DDLFunction.SelectedValue)
            {
                case "sin": return Math.Sin(TBoxValue);
                case "cos": return Math.Cos(TBoxValue);
                case "tg" : return Math.Tan(TBoxValue);
                case ""   : return (TBoxValue);
                //case "ctg": return Math. IValue;
                default: return 0;
            }
        }
        //Метод для обнавления выхода контрола
        public void UpdateOValue()
        {
            if (double.TryParse(TBox.Text, out TBoxValue))
                OValue = GetOperationResult(GetFunctionResult());
            else
            {
                OValue = 0;
                ErrorString = "Введите число.";
            }
        }
        //Событие изменения содержания TextBox-а
        protected void TextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateOValue();
        }
        //Событие изменения выбора DropDownList-а Функций
        protected void DDLFunction_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOValue();
        }
        //Событие изменения выбора DropDownList-а Операций
        protected void DDLOperation_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateOValue();
        }
        //Конструктор
        public CalcOperation(int _CNum)
        {
            Controls.Clear();
            CNum = _CNum;

            DDLOperation = new DropDownList();
            DDLOperation.ID = "DDLOperation" + CNum;
            DDLOperation.Width = 60;
            DDLOperation.DataSource = new[] { "+", "-", "*", "/", "^" };
            DDLOperation.DataBind();
            DDLOperation.SelectedIndexChanged += new EventHandler(DDLOperation_SelectedIndexChanged);
            DDLOperation.AutoPostBack = true;

            DDLFunction = new DropDownList();
            DDLFunction.ID = "DDLFunction" + CNum;
            DDLFunction.Width = 60;
            DDLFunction.DataSource = new[] { "", "sin", "cos", "tg"};
            DDLFunction.DataBind();
            DDLFunction.SelectedIndexChanged += new EventHandler(DDLFunction_SelectedIndexChanged);
            DDLFunction.AutoPostBack = true;

            TBox = new TextBox();
            TBox.ID = "TBox" + CNum;
            TBox.Width = 120;
            TBox.TextChanged += new EventHandler(TextBox_TextChanged);
            TBox.AutoPostBack = true;            

            this.Controls.Add(DDLOperation);
            this.Controls.Add(DDLFunction);
            this.Controls.Add(TBox);
        }

        protected override void CreateChildControls()
        {
            Controls.Clear();

            this.Controls.Add(DDLOperation);
            this.Controls.Add(DDLFunction);
            this.Controls.Add(TBox);

            base.CreateChildControls();
        }
        
        //Рендер контрола
        protected override void Render(HtmlTextWriter writer)
        {
            //AddAttributesToRender(writer);

            writer.RenderBeginTag(HtmlTextWriterTag.Table);

            writer.RenderBeginTag(HtmlTextWriterTag.Tr);
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "70px");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    DDLOperation.RenderControl(writer);
                writer.RenderEndTag();
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "70px");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    DDLFunction.RenderControl(writer);
                writer.RenderEndTag();
                writer.AddStyleAttribute(HtmlTextWriterStyle.Width, "130px");
                writer.RenderBeginTag(HtmlTextWriterTag.Td);
                    TBox.RenderControl(writer);
                writer.RenderEndTag();
            writer.RenderEndTag();
        }
        

        //3 метода для сохранения и загрузки состояния контрола
        protected override void OnInit(EventArgs e)
        {
            Page.RegisterRequiresControlState(this);
            base.OnInit(e);
        }

        protected override object SaveControlState()
        {
            object baseState = base.SaveControlState();

            //create an array to hold the base control’s state 
            //and this control’s state.
            object thisState = new object[] { baseState, 
                                              this.IValue, 
                                              this.OValue, 
                                              this.DDLFunction.SelectedIndex,
                                              this.DDLOperation.SelectedIndex,
                                              this.TBox.Text};
            return thisState;
        }

        protected override void LoadControlState(object state)
        {
            object[] stateLastRequest = (object[])state;

            //Grab the state for the base class 
            //and give it to it.
            object baseState = stateLastRequest[0];
            base.LoadControlState(baseState);

            //Now load this control’s state.
            IValue = (double)stateLastRequest[1];
            OValue = (double)stateLastRequest[2];
            this.DDLFunction.SelectedIndex = (int)stateLastRequest[3];
            this.DDLOperation.SelectedIndex = (int)stateLastRequest[4];
            this.TBox.Text = (string)stateLastRequest[5];
        }

    }
}