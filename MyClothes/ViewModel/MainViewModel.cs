using GalaSoft.MvvmLight;
using MyClothes.Model;
using System;
using System.Collections.ObjectModel;

namespace MyClothes.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        // <summary>
        /// The <see cref="DayList" /> property's name.
        /// </summary>
        public const string DayListPropertyName = "DayList";

        private ObservableCollection<Day> _dayList = null;

        /// <summary>
        /// Sets and gets the DayList property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<Day> DayList
        {
            get
            {
                return _dayList;
            }

            set
            {
                if (_dayList == value)
                {
                    return;
                }

                //RaisePropertyChanging(DayListPropertyName);
                _dayList = value;
                RaisePropertyChanged(DayListPropertyName);
            }
        }
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            if (IsInDesignMode)
            {
                DayList = new ObservableCollection<Day>();
                DayList.Add(new Day{temp=20,Time=DateTime.Now});
                DayList.Add(new Day { temp = 20, Time = DateTime.Now.AddDays(1) });
                DayList.Add(new Day { temp = 20, Time = DateTime.Now.AddDays(2) });
                DayList.Add(new Day { temp = 20, Time = DateTime.Now.AddDays(3) });
                DayList.Add(new Day { temp = 20, Time = DateTime.Now.AddDays(4) });
            }
            else
            {
                // Code runs "for real"
            }
        }
    }
}