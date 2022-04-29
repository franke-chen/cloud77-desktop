using Cloud77.Middleware;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloud77.WPF.ViewModel
{
    public class WorkItem
    {
        public string Name { get; set; }
    }

    public class HomeViewModel: BaseViewModel
    {
        public HomeViewModel()
        {
            Items = new ObservableCollection<WorkItem>();

            Items.Add(new WorkItem() { Name = "item1" });
            Items.Add(new WorkItem() { Name = "item2" });
            Items.Add(new WorkItem() { Name = "item3" });
            Items.Add(new WorkItem() { Name = "item4" });
            Items.Add(new WorkItem() { Name = "item5" });

            var tester = new TesterRepository().GetTester();
            if (tester != null)
            {
                Items.Add(new WorkItem() { Name = tester.Email });
            }
        }

        public ObservableCollection<WorkItem> Items { get; set; }
    }
}
