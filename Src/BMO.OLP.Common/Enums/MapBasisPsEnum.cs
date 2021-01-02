using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMO.OLP.Common.Enums
{
  public class MapBasis
  {
    public static MapBasis J77H = new MapBasis("7"); // dt77: High proirity from EM (overwrites FLEI)
    public static MapBasis J77g = new MapBasis("8"); // dt77: High proirity from EM (overwrites FLEI)  --- overwrote non-null mapbasis
    public static MapBasis AllB = new MapBasis("a"); // Show all regardless of matching etc.
    public static MapBasis Conn = new MapBasis("c"); // connection from STG_ADAPTIV_CUSTOMER or BBG from CCBasel.CM_ [JH_] UEN_MAPPING
    public static MapBasis UnMp = new MapBasis("d"); // Mapping deleted by the user
    public static MapBasis Dt17 = new MapBasis("e"); // ADAPTIV "STG_ADAPTIV_CUSTOMER.CUST_UEN has a match in LGL_ENTIY by UEN"
    public static MapBasis HiFg = new MapBasis("g"); // ** Matching algo: Perfect Match. Mapped by High Fidelity rule #2: CORE name + country
    public static MapBasis HiFi = new MapBasis("h"); // ** Matching algo: Perfect Match. Mapped by High Fidelity rule #1: Legal/Company name + country
    public static MapBasis J77L = new MapBasis("i"); // dt77: Low proirity from AE.LEId == LE.Id
    public static MapBasis J77k = new MapBasis("k"); // dt77: Low proirity from AE.LEId == LE.Id       --- overwrote non-null mapbasis
    public static MapBasis LoFi = new MapBasis("l"); // ** Matching algo: Low Fidelity
    public static MapBasis PMgr = new MapBasis("p"); // Manual Prod Manager matching
    public static MapBasis User = new MapBasis("u"); // Manual User matching
    public static MapBasis ExaC = new MapBasis("x"); // ** Matching algo: Exact Connection (matching step #2)
    public static MapBasis YtUk = new MapBasis("y"); // Yet UnKnown, UnMapped, New.
    public static MapBasis Zero = new MapBasis("z"); // ** Matching algo: zero found

    public static string[] UIList = new string[] { AllB.Id, Conn.Id, HiFi.Id, LoFi.Id, UnMp.Id, User.Id,          Zero.Id };
    public static string[] ToBeFedToAlgos = new string[] {                    LoFi.Id,                   YtUk.Id, Zero.Id };
    private MapBasis(string name) { Id = name; }
    public string Id { get; private set; }
    public override string ToString() { return Id; }
  }
}
