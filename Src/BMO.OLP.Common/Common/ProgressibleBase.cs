using System;
using System.Threading.Tasks;
using BMO.OLP.Interfaces;

namespace BMO.OLP.Common
{
  public abstract class ProgressibleBase : IProgressible
	{
		protected IFeedProcessor _fpm;

		public abstract Task Start(DateTime batchT, bool isFull = false);
		public void Stop() { throw new NotImplementedException(string.Format("Under construction: {0}.{1}() ... not for this release.", System.Reflection.MethodInfo.GetCurrentMethod().DeclaringType.Name, System.Reflection.MethodInfo.GetCurrentMethod().Name));; }
		public double MaximumPosition { get { return _fpm == null ? 0d : _fpm.MaxFpm; } set { } }
		public double CurrentPosition { get { return _fpm == null ? 0d : _fpm.CurFpm; } set { } }
		public string ProcessLogStory { get { return _fpm == null ? "" : _fpm.PrgLog; } set { } }
		public string Descrn { get; set; }
		public string Report { get; set; }
		public DateTime BatchTm { get; set; }
	}
}
