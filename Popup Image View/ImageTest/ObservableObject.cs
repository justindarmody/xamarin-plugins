using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using PropertyChanged;

namespace ImageTest
{
	[ImplementPropertyChanged]
	public class ObservableObject
	{
		/// <summary>
		/// Occurs when property changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		protected ObservableObject()
		{
		}

		/// <summary>
		/// Raise the property changed event for this class
		/// </summary>
		/// <param name="propertyName">The name of the property to notify changed</param>
		protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = null)
		{
			if (this.PropertyChanged != null)
			{
				this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		public void ForceRaisePropertyChanged([CallerMemberName]string propertyName = null)
		{
			this.RaisePropertyChanged(propertyName);
		}
	}
}