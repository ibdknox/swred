
using System;
namespace HoldItCore {
	public static class Utils {

		private static Random rng = new Random();

		public static Random RNG {
			get { return Utils.rng; }
		}
	}
}
