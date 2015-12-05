using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace WebApplication5.Models {
    public class CalendarModel {

        public List<SelectListItem> hourList = new List<SelectListItem>();
        public int startHourVal { get; set; }
        public int endHourVal { get; set; }

        public List<SelectListItem> minuteList = new List<SelectListItem>();
        public int startMinuteVal { get; set; }
        public int endMinuteVal { get; set; }
        
        public List<SelectListItem> timeframeList = new List<SelectListItem>();
        public int startTimeframeVal { get; set; }
        public int endTimeframeVal { get; set; }

        [HiddenInput(DisplayValue = false)]
        public string startMinuteText { get; set; }
        public string endMinuteText { get; set; }
        public string startTimeframeText { get; set; }
        public string endTimeframeText { get; set; }


        public void populateLists() {
            var data = new[]{
                new SelectListItem { Value = "1", Text = "1" },
                new SelectListItem { Value = "2", Text = "2" },
                new SelectListItem { Value = "3", Text = "3" },
                new SelectListItem { Value = "4", Text = "4" },
                new SelectListItem { Value = "5", Text = "5" },
                new SelectListItem { Value = "6", Text = "6" },
                new SelectListItem { Value = "7", Text = "7" },
                new SelectListItem { Value = "8", Text = "8" },
                new SelectListItem { Value = "9", Text = "9" },
                new SelectListItem { Value = "10", Text = "10" },
                new SelectListItem { Value = "11", Text = "11" },
                new SelectListItem { Value = "12", Text = "12" },
            };
            hourList = data.ToList();

            data = new[]{
                new SelectListItem { Value = "1", Text = "00" },
                new SelectListItem { Value = "2", Text = "15" },
                new SelectListItem { Value = "3", Text = "30" },
                new SelectListItem { Value = "4", Text = "45" },
            };
            minuteList = data.ToList();

            data = new[]{
                new SelectListItem{ Value="1",Text="AM"},
                new SelectListItem{ Value="2",Text="PM"},
            };
            timeframeList = data.ToList();
        }
    }
}