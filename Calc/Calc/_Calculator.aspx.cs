using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace Calc
{
   public partial class _Calculator : System.Web.UI.Page
    {
        //Лист хронящий контролы операций
        private List<CalcOperation> ListCalcOperation = new List<CalcOperation>();
        //количество добавленных контролов
        private int n
        {
            get
            {
                object o = ViewState["N_controls"];
                return (o == null) ? 0 : (int)o;
            }

            set
            {
                ViewState["N_controls"] = value;
            }
        }        

        //Создание контролов
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                n = 0;
                ListCalcOperation.Add(new CalcOperation(0));
                PlaceHolder1.Controls.Add(ListCalcOperation[0]);
            }
            else
            {
                for (int i = 0; i <= n; i++)
                {
                    ListCalcOperation.Add(new CalcOperation(i));
                }

                foreach (CalcOperation ListCO in ListCalcOperation)
                {
                    PlaceHolder1.Controls.Add(ListCO);
                }

            }          
        }
        
       //Обнавление значений контролов
        protected void Page_PreRender(object sender, EventArgs e)
        {
            string err=null;

            if (IsPostBack)
            {
                //обнавление входного значения 0-го контрола
                double inputValue;
                
                if (double.TryParse(TextBox1.Text, out inputValue))
                { ListCalcOperation[0].IValue = inputValue; }
                else
                {
                    ListCalcOperation[0].IValue = 0;
                };

                //обнавление входных значений для остальных контролов
                for (int i = 0; i < n; i++)
                {
                    ListCalcOperation[i].UpdateOValue();
                    ListCalcOperation[i + 1].IValue = ListCalcOperation[i].GetOValue();
                    if (ListCalcOperation[i].ErrorStr!=null)
                        err = ListCalcOperation[i].ErrorStr;
                }
                ListCalcOperation[n].UpdateOValue();
            }
            
            if (ListCalcOperation[n].ErrorStr != null)
                err = ListCalcOperation[n].ErrorStr;

            if (err == null)
                Label1.Text = "Результат: " + ListCalcOperation[n].GetOValue().ToString();
            else
                Label1.Text = err;
        }

       //Событие нажатия кнопки добавления контрола
        protected void Button1_Click(object sender, EventArgs e)
        {
            ++n;
            
            ListCalcOperation.Add(new CalcOperation(n));
            PlaceHolder1.Controls.Add(ListCalcOperation[n]);
        }

       //Событие изменения текста TextBox-а содержащего исходное значение
        protected void TextBox1_TextChanged(object sender, EventArgs e)
        {
            double inputValue;

            if (double.TryParse(TextBox1.Text, out inputValue) != true)
            {
                TextBox1.Text = "0";
            };
        }



    }
}