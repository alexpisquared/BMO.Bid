using System.Windows.Media;

namespace BMO.OLP.FeedWatcher.App.Core
{
	public class StatusPseudoEnum
	{
		public static StatusPseudoEnum BrandNewFile = new StatusPseudoEnum("New File", Color.FromRgb(0, 164, 0), Color.FromRgb(164, 255, 164));
		public static StatusPseudoEnum FoundMissing = new StatusPseudoEnum("Missing", Colors.Blue, Colors.LightBlue);
		public static StatusPseudoEnum ToBeProcessed = new StatusPseudoEnum("To be Processed", Colors.Brown, Colors.Orange);
		public static StatusPseudoEnum IsBeingProcessed = new StatusPseudoEnum("Is being Processed", Color.FromRgb(164, 164, 0), Color.FromRgb(255, 255, 164));
		public static StatusPseudoEnum HasBeenProcessed = new StatusPseudoEnum("Has been Processed", Colors.Gray, Color.FromRgb(233, 233, 233));
		public static StatusPseudoEnum EXEPTION_Generic = new StatusPseudoEnum("EXEPTION: Something happened.", Color.FromRgb(255, 0, 255), Color.FromRgb(255, 128, 255));
		public static StatusPseudoEnum EXEPTION_Deleted = new StatusPseudoEnum("EXEPTION: Deleted before processed.", Colors.Red, Colors.LightPink);

		private StatusPseudoEnum(string name, Color color, Color color2) { Name = name; Color1 = color; Color2 = color2; }
		public string Name { get; private set; }
		public Color Color1 { get; private set; }
		public Color Color2 { get; private set; }
		public Brush Brush
		{
			get
			{
				var rv = new RadialGradientBrush();// { GradientOrigin = Point(0.3, 0.3) };
				rv.GradientStops.Add(new GradientStop(Color1, 0));
				rv.GradientStops.Add(new GradientStop(Color2, 1));
				//???rv.GradientOrigin.X = .3;
				return rv;
			}
		}
		public override string ToString() { return Name; }

	}
}
