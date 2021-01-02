using AsLink;
using System;
using System.Diagnostics;
using System.DirectoryServices;
using System.Reflection;

namespace BMO.OLP.Common.Common
{
	public class LdapHelper
	{
		public static void GetUserDetails(string user, out string name, out string mail, out string ueid, out string dprt, out string note)
		{
			mail = name = ueid = dprt = note = null;
			try
			{
				var entry1 = new DirectoryEntry("LDAP://RootDSE");												// Get the currently connected LDAP context 
				var dmnCtx = entry1.Properties["defaultNamingContext"].Value as string;
				var entry2 = new DirectoryEntry("LDAP://" + dmnCtx);											// Use the default naming context as the connected context may not work for searches
				var adSrch = new DirectorySearcher(entry2);

				if (user.Contains("\\"))
					user = user.Split('\\')[1];

				adSrch.Filter = string.Format("(&(objectClass=user)(anr={0}))", user);

				foreach (SearchResult singleADUser in adSrch.FindAll())										// Go through all entries from the active directory.
				{
					foreach (string singleAttribute in ((ResultPropertyCollection)singleADUser.Properties).PropertyNames)				// Go through all the values found in the search
					{
						if (singleAttribute == "name")						/**/ name = singleADUser.Properties[singleAttribute][0].ToString();
						if (singleAttribute == "mail")						/**/ mail = singleADUser.Properties[singleAttribute][0].ToString();
						if (singleAttribute == "employeeid")			/**/ ueid = singleADUser.Properties[singleAttribute][0].ToString();
						if (singleAttribute == "department")			/**/ dprt = singleADUser.Properties[singleAttribute][0].ToString();
						//if (singleAttribute == "accountexpires")	/**/ expn = singleADUser.Properties[singleAttribute][0].ToString(); // new DateTime(1601, 01, 02).AddTicks(expn); //var ldapTicks = long.Parse(expn);						var dt = ldapTicks.Equals(Int64.MaxValue) ? DateTime.MaxValue : DateTime.FromFileTime(ldapTicks); Debug.WriteLine("{0,32} · {1}", singleAttribute, singleADUser.Properties[singleAttribute][0]);
					}
				}
				note = "LDAP provided. " + Assembly.GetEntryAssembly().GetName().Name;
			}
			catch (Exception ex)
			{
				DevOp.ExHrT(ex, System.Reflection.MethodInfo.GetCurrentMethod());
				note = string.Format("Problems getting LDAP info for user {0}: {1}", user, ex.Message);
			}
		}

		static void listAllUserDetails(string user)
		{
			var entry1 = new DirectoryEntry("LDAP://RootDSE");												// Get the currently connected LDAP context 
			var dmnCtx = entry1.Properties["defaultNamingContext"].Value as string;
			var entry2 = new DirectoryEntry("LDAP://" + dmnCtx);											// Use the default naming context as the connected context may not work for searches
			var adSrch = new DirectorySearcher(entry2);

			adSrch.Filter = string.Format("(&(objectClass=user)(anr={0}))", user);

			foreach (SearchResult singleADUser in adSrch.FindAll())										// Go through all entries from the active directory.
			{
				Debug.WriteLine("The properties of the  [" + singleADUser.GetDirectoryEntry().Name + "]  are :");
				foreach (string singleAttribute in ((ResultPropertyCollection)singleADUser.Properties).PropertyNames)				// Go through all the values found in the search
				{
					Debug.WriteLine(singleAttribute + " = ", "  ");
					foreach (Object singleValue in ((ResultPropertyCollection)singleADUser.Properties)[singleAttribute])
					{
						Debug.WriteLine("\t\t" + singleValue);
					}
				}
			}
		}
	}
}
