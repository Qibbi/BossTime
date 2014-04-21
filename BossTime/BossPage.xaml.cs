using System;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Threading;

namespace BossTime
{
	public partial class BossPage : UserControl
	{
		private readonly DispatcherTimer _timer;
		private ObservableCollection<Boss> _bossCollection;

		public BossPage()
		{
			InitializeComponent();
			_bossCollection = new ObservableCollection<Boss>();
			for (int idx = 0; idx < Boss.Bosses.Length; ++idx)
			{
				_bossCollection.Add(Boss.Bosses[idx]);
			}
			BossList.ItemsSource = _bossCollection;
			_timer = new DispatcherTimer();
			_timer.Tick += new EventHandler(TimerTick);
			_timer.Start();
		}

		private void TimerTick(object sender, EventArgs args)
		{
			Boss.Refresh();
			_bossCollection.InsertionSort();
		}
	}
}
