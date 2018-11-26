using System;
using Xamarin.Forms;

namespace EGuardian.Controls
{
    public class ExtendedDateTime : StackLayout
    {
        private readonly ExtendedDatePicker _date;
        public readonly ExtendedTimePicker _time;


        public ExtendedDateTime(ExtendedDatePicker date, ExtendedTimePicker time)
        {
            _date = date;
            _time = time;
        }


        #region Value

        public static readonly BindableProperty ValueProperty =
            BindableProperty.Create<ExtendedDateTime, DateTime>(p => p.Value, default(DateTime));

        public DateTime Value
        {
            get
            {
                return _date.Date.Add(_time.Time);
            }
            set
            {
                _date.SetValue(DatePicker.DateProperty, value);
                _time.SetValue(TimePicker.TimeProperty, value);
            }
        }

        #endregion
    }
}