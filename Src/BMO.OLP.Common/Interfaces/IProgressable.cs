using System;
using System.Threading.Tasks;

namespace BMO.OLP.Interfaces
{
	public interface IProgressible
	{
		Task Start(DateTime batchT, bool isFull = false);
		void Stop();
		double MaximumPosition { get; set; }
		double CurrentPosition { get; set; }
		string ProcessLogStory { get; set; }
		string Descrn { get; set; }
		string Report { get; set; }
		DateTime BatchTm { get; set; }
	}
}
