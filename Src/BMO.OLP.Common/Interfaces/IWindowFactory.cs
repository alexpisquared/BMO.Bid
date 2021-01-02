using BMO.OLP.UenMatcher.Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace BMO.OLP.Interfaces
{
  public interface IMvvmWindowFactory { void ShowNewWindow(); }
  public interface IMvvmVmWindowFactory { void ShowNewWindow(INotifyPropertyChanged vm); }
  public interface IMvvmVVWindowFactory { void ShowNewWindow(INotifyPropertyChanged vm, UIElement vw); }
  public interface IAdmWgtWindowFactory { void ShowNewWindow(object stuff, bool isModal = false); }
  public interface IStrLstWindowFactory { void ShowNewWindow(string srcName, List<string> nameList, bool isModal = false); }
  public interface IStrLstWindowFactoryRC { void ShowNewWindow(IEnumerable<WordStat> ws, bool isModal = false); }
}
