using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Media.Imaging;

namespace BossTime
{
	public class Boss : INotifyPropertyChanged, IComparable
	{
		public event PropertyChangedEventHandler PropertyChanged;

		public string Name { get; private set; }
		public BitmapImage Image { get; private set; }

		public TimeSpan[] TriggerTimes { get; private set; }

		private string _description;
		private string _descriptionActive;
		private string _descriptionFinal;
		private bool _isActive;
		private double _progression;
		private string _progressionText;

		public static Boss[] Bosses { get; set; }

		private static readonly TimeSpan _bossTimer;
		private static readonly Uri _currentUri;

		public string Description
		{
			get
			{
				return _descriptionFinal;
			}
			private set
			{
				_descriptionFinal = string.Format("{0}\n[{1}]", ((IsActive) ? _descriptionActive : _description), value);
				OnPropertyChanged("Description");
			}
		}

		public bool IsActive
		{
			get
			{
				return _isActive;
			}
			set
			{
				_isActive = value;
				OnPropertyChanged("IsActive");
			}
		}

		public double Progression
		{
			get
			{
				return _progression;
			}
			set
			{
				_progression = value;
				OnPropertyChanged("Progression");
			}
		}

		public string ProgressionText
		{
			get
			{
				return _progressionText;
			}
			set
			{
				_progressionText = value;
				OnPropertyChanged("ProgressionText");
			}
		}

		static Boss()
		{
			_bossTimer = new TimeSpan(0, 15, 0);
			_currentUri = new Uri(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location + Path.DirectorySeparatorChar));
			Bosses = new Boss[]
			{
				new Boss("Thaumanova Reactor Fallout",
					"Rooba is prepairing for future clean-up efforts.\nThere are currently no active operations within the reactor.",
					"Destroy the fire elemental created from chaotic energy fusing with the C.L.E.A.N. 5000's energy core.",
					"FireElemental",
					new TimeSpan(0, 45, 0), 120),
				new Boss("The Battle for Wychmire Swamp",
					"The forces of darkness have been driven back. An uneasy calm has fallen over the swamp.",
					"Defeat the great jungle wurm.",
					"JungleWurm",
					new TimeSpan(1, 15, 0), 120),
				new Boss("Secrets in the Swamp",
					"The swamp lies dormant.",
					"The beast has been awakened!\nDefeat the shadow behemoth.",
					"ShadowBehemoth",
					new TimeSpan(1, 45, 0), 120),
				new Boss("The Frozen Maw",
					"The Maw is quiet.",
					"Kill the Svanir shaman chief to break his control over the ice elemental.",
					"SvanirShaman",
					new TimeSpan(0, 15, 0), 120),
					
				new Boss("The Battle for Mount Maelstrom",
					"The volcano lies silent.",
					"The volcano is active. ",
					"Megadestroyer",
					new TimeSpan[]
					{
						new TimeSpan(1, 0, 0), new TimeSpan(6, 0, 0), new TimeSpan(11, 30, 0), new TimeSpan(16, 30, 0), new TimeSpan(21, 30, 0)
					}),
				new Boss("The Campaign Against Taidha Covington",
					"Taidha Covington has been defeated, and Laughing Gull Island is controlled by the Lionguard.",
					"Taidha Covington is inside her keep, making her last stand against the Lionguard.\nKill Admiral Taidha Covington.",
					"TaidhaCovington",
					new TimeSpan[]
					{
						new TimeSpan(0, 30, 0), new TimeSpan(5, 30, 0), new TimeSpan(11, 0, 0), new TimeSpan(16, 0, 0), new TimeSpan(21, 0, 0)
					}),

				new Boss("Breaking the Claw of Jormag",
					"The sky is calm. Jormag's lieutenant has been driven from Frostgorge Sound.",
					"By stopping its corruptive crystals, the Pact has lured a Claw of Jormag from the sky. Now, everyone must rally to defeat the beast.\nDefeat the Claw of Jormag",
					"ClawOfJormag",
					new TimeSpan[]
					{
						new TimeSpan(0, 0, 0), new TimeSpan(4, 30, 0), new TimeSpan(8, 30, 0), new TimeSpan(10, 30, 0), new TimeSpan(15, 30, 0), new TimeSpan(20, 30, 0)
					}),
				new Boss("Golem Mark II",
					"The Golem Mark II has been defeated!",
					"Defeat the Inquest's Golem Mark II.",
					"GolemMarkII",
					new TimeSpan[]
					{
						new TimeSpan(3, 30, 0), new TimeSpan(8, 0, 0), new TimeSpan(10, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(19, 30, 0), new TimeSpan(23, 30, 0)
					}),
				new Boss("Seraph Assault on Centaur Camps",
					"The Seraph control all centaur camps!",
					"Defeat Ulgoth the Modniir and his minions.",
					"ModniirUlgoth",
					new TimeSpan[]
					{
						new TimeSpan(2, 30, 0), new TimeSpan(7, 30, 0), new TimeSpan(9, 30, 0), new TimeSpan(14, 0, 0), new TimeSpan(18, 30, 0), new TimeSpan(23, 0, 0)
					}),
				new Boss("Kralkatorrik's Legacy",
					"The Shatterer has been defeated! Vigil and Sentinel forces will continue working on contain the threats posed by Kralkatorrik's minions.",
					"Slay the Shatterer",
					"TheShatterer",
					new TimeSpan[]
					{
						new TimeSpan(2, 0, 0), new TimeSpan(7, 0, 0), new TimeSpan(9, 0, 0), new TimeSpan(13, 0, 0), new TimeSpan(17, 30, 0), new TimeSpan(22, 30, 0)
					}),
					
				new Boss("Triple Trouble",
					"The coast is clear of great jungle wurm attacks.",
					"The three heads of a great jungle wurm are attacking Bloodtide Coast!",
					"GreatJungleWurm",
					new TimeSpan[]
					{
						new TimeSpan(5, 0, 0), new TimeSpan(14, 30, 0), new TimeSpan(20, 0, 0)
					}),
				new Boss("Island Control",
					"",
					"Defeat the Karka Queen threatening the settlements.",
					"KarkaQueen",
					new TimeSpan[]
					{
						new TimeSpan(3, 0, 0), new TimeSpan(12, 30, 0), new TimeSpan(18, 0, 0)
					}),
				new Boss("Danger at Fabled Djannor",
					"The shore is calm.",
					"Tequatl is attacking!\nDefeat Tequatl the Sunless.",
					"Tequatl", 
					new TimeSpan[]
					{
						new TimeSpan(4, 0, 0), new TimeSpan(13, 30, 0), new TimeSpan(19, 0, 0)
					}),

				new Boss("[Empty Slot]",
					"No Boss active!",
					"No Boss active!",
					"TBD",
					new TimeSpan[]
					{
						new TimeSpan(1, 30, 0), new TimeSpan(6, 30, 0), new TimeSpan(12, 0, 0), new TimeSpan(17, 0, 0), new TimeSpan(22, 0, 0)
					})
			};
		}

		public Boss(string name, string description, string descriptionActive, string imageName, TimeSpan[] triggerTimes)
		{
			Name= name;
			_description = description;
			_descriptionActive = descriptionActive;
			Uri imageUri = new Uri(_currentUri, string.Format("Art{0}Textures{0}Bosses{0}{1}.png", Path.DirectorySeparatorChar, imageName));
			if (File.Exists(imageUri.LocalPath))
			{
				Image = new BitmapImage(imageUri);
			}
			else
			{
				Image = new BitmapImage();
			}
			TriggerTimes = triggerTimes;
		}

		public Boss(string name, string description, string descriptionActive, string imageName, TimeSpan firstTrigger, int minsToPass)
		{
			Name = name;
			_description = description;
			_descriptionActive = descriptionActive;
			Uri imageUri = new Uri(_currentUri, string.Format("Art{0}Textures{0}Bosses{0}{1}.png", Path.DirectorySeparatorChar, imageName));
			if (File.Exists(imageUri.LocalPath))
			{
				Image = new BitmapImage(imageUri);
			}
			else
			{
				Image = new BitmapImage();
			}
			TriggerTimes = new TimeSpan[((24 - firstTrigger.Hours) * 60 - firstTrigger.Minutes) / minsToPass + 1];
			for (int idx = 0; idx < TriggerTimes.Length; ++idx)
			{
				TriggerTimes[idx] = firstTrigger;
				firstTrigger += new TimeSpan(minsToPass / 60, minsToPass % 60, 0);
			}
		}

		public static void Refresh()
		{
			for (int idx = 0; idx < Bosses.Length; ++idx)
			{
				Bosses[idx].RefreshInstance();
			}
		}

		private void RefreshInstance()
		{
			TimeSpan timeNow = DateTime.Now.TimeOfDay;
			bool isActive = false;
			for (int idx = 0; idx < TriggerTimes.Length; ++idx)
			{
				if (TriggerTimes[idx] <= timeNow && TriggerTimes[idx] + _bossTimer > timeNow)
				{
					isActive = true;
					break;
				}
			}
			if (isActive != IsActive)
			{
				IsActive = isActive;
				Description = string.Empty;
			}
			if (isActive)
			{
				for (int idx = 0; idx < TriggerTimes.Length; ++idx)
				{
					if (TriggerTimes[idx] <= timeNow && TriggerTimes[idx] + _bossTimer > timeNow)
					{
						Progression = (timeNow - TriggerTimes[idx]).TotalMilliseconds / _bossTimer.TotalMilliseconds * 100.0d;
						ProgressionText = (timeNow - TriggerTimes[idx] - _bossTimer).Negate().ToString();
						Description = string.Format("{0}", (timeNow - TriggerTimes[idx] - _bossTimer).Negate());
						break;
					}
				}
			}
			else
			{
				int timeFound = -1;
				for (int idx = 0; idx < TriggerTimes.Length; ++idx)
				{
					if (TriggerTimes[idx] > timeNow)
					{
						timeFound = idx;
						break;
					}
				}
				if (timeFound == -1)
				{
					timeFound = 0;
					timeNow -= new TimeSpan(24, 0, 0);
				}
				Progression = 0.0d;
				ProgressionText = string.Format("{0}", (TriggerTimes[timeFound] - timeNow).ToString());
				Description = TriggerTimes[timeFound].ToString();
			}
		}

		private void OnPropertyChanged(string source)
		{
			PropertyChangedEventHandler handler = PropertyChanged;
			if (handler != null)
			{
				handler(this, new PropertyChangedEventArgs(source));
			}
		}

		public int CompareTo(object obj)
		{
			if (IsActive)
			{
				return -1;
			}
			Boss other = obj as Boss;
			if (other == null)
			{
				return 0;
			}
			if (other.IsActive)
			{
				return 1;
			}
			TimeSpan timeNow = DateTime.Now.TimeOfDay;
			TimeSpan[] otherTriggerTimes = other.TriggerTimes;
			int nextTrigger = -1;
			int otherNextTrigger = -1;
			TimeSpan nextTriggerTime = new TimeSpan(0, 0, 0);
			TimeSpan otherNextTriggerTime = new TimeSpan(0, 0, 0);
			for (int idx = 0; idx < TriggerTimes.Length; ++idx)
			{
				if (TriggerTimes[idx] > timeNow)
				{
					nextTrigger = idx;
					break;
				}
			}
			if (nextTrigger == -1)
			{
				nextTrigger = 0;
				nextTriggerTime = new TimeSpan(24, 0, 0);
			}
			nextTriggerTime += TriggerTimes[nextTrigger];
			for (int idx = 0; idx < otherTriggerTimes.Length; ++idx)
			{
				if (otherTriggerTimes[idx] > timeNow)
				{
					otherNextTrigger = idx;
					break;
				}
			}
			if (otherNextTrigger == -1)
			{
				otherNextTrigger = 0;
				otherNextTriggerTime = new TimeSpan(24, 0, 0);
			}
			otherNextTriggerTime += otherTriggerTimes[otherNextTrigger];
			return nextTriggerTime.CompareTo(otherNextTriggerTime);
		}
	}
}
