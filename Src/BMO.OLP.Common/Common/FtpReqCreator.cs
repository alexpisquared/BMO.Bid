using System.Collections.Generic;
using System.Text;
using BMO.OLP.Common.Enums;

namespace OLP.DAQ.Ftp.Skyway.Core
{
  public class ReqCreator
  {
    public static string CreateRequest(List<string> ids, SecIdTypePsEnum idType, string replyFilename, bool useCache)
    {
      var sb = new StringBuilder();
      
      sb.Append(string.Format(ReqHeader, replyFilename, useCache ? CacheOpt : ""));

#if PROD
			ids.Take(2).ToList().ForEach(id => sb.Append(id).Append("|").Append(idType).Append("\r\n"));
#else
      ids.ForEach(id => sb.Append(id).Append("|").Append(idType).Append("\r\n"));
#endif

      sb.Append(ReqFooter);

      return sb.ToString();
    }
    public static string FtpResponse_Sample { get { return OutSample; } }

    #region Strings //todo: Move to the SQL store.
    const string ReqHeader = @"
START-OF-FILE

FIRMNAME=dl87543

COLUMNHEADER=yes

HEADER=Yes

# default delimiter 
DELIMITER=|

REPLYFILENAME={0}.out

PROGRAMNAME=getdata

PROGRAMFLAG=one-shot

# DERIVED=yes

# CLOSINGVALUES=yes

SECMASTER=yes

# I suspect the next 3 fields (USERNAME, SN, WS ) are not required for pure security description attributes (SECMASTER) requests
USERNUMBER=5262715

SN=185383

WS=0

{1}

START-OF-FIELDS

ID_BB_GLOBAL
ID_ISIN
ID_CUSIP
ID_SEDOL1
TICKER
EXCH_CODE
TICKER_AND_EXCH_CODE
SECURITY_DES
MARKET_SECTOR_DES
MATURITY
ISSUER_INDUSTRY
INDUSTRY_SECTOR
INDUSTRY_GROUP
INDUSTRY_SUBGROUP
SECURITY_TYP
SECURITY_TYP2
ID_BB_COMPANY
ISSUER

END-OF-FIELDS

# setting SECID to one of the eligible values, assigns a default such that do not have to specify on
# each row in START-OF-DATA block the the tyhpe of security ID being used to fetch by. 
# Can still override defualt value set in SECID statement by specifiying on rows not using the default value
# see page 29 of Bloomberg Persecurity_product_manual.pdf

#SECID=ISIN
#SECID=CUSIP
#SECID=SEDOL

START-OF-DATA
",
 CacheOpt = @"
# Enable BMO Skyway daily cache -- default is yes
X_DATA_SRC_CACHE=yes
# Disable BBG requests -- default is yes
X_DATA_SRC_DIRECT=no
# Enable BMO Skyway back office optimization -- default is no
X_DATA_SRC_HIST=yes
",
 OutSample = @"
START-OF-FILE

FIRMNAME=dl87543
COLUMNHEADER=yes
HEADER=Yes
DELIMITER=|
REPLYFILENAME=A0_OLP_Mon0.out
PROGRAMNAME=getdata
PROGRAMFLAG=one-shot
SECMASTER=yes
USERNUMBER=5262715
SN=185383
WS=0

START-OF-FIELDS
ID_BB_GLOBAL
ID_ISIN
ID_CUSIP
ID_SEDOL1
TICKER
EXCH_CODE
TICKER_AND_EXCH_CODE
SECURITY_DES
MARKET_SECTOR_DES
MATURITY
ISSUER_INDUSTRY
INDUSTRY_SECTOR
INDUSTRY_GROUP
INDUSTRY_SUBGROUP
SECURITY_TYP
SECURITY_TYP2
ID_BB_COMPANY
ISSUER
END-OF-FIELDS

TIMESTARTED=Wed Feb 25 14:26:48 EST 2015
START-OF-DATA
SECURITIES|ERROR CODE|NUM FLDS|ID_BB_GLOBAL|ID_ISIN|ID_CUSIP|ID_SEDOL1|TICKER|EXCH_CODE|TICKER_AND_EXCH_CODE|SECURITY_DES|MARKET_SECTOR_DES|MATURITY|ISSUER_INDUSTRY|INDUSTRY_SECTOR|INDUSTRY_GROUP|INDUSTRY_SUBGROUP|SECURITY_TYP|SECURITY_TYP2|ID_BB_COMPANY|ISSUER|
VG0985293088|0       |16      |BBG000F640B2|VG09852|09859308|2110510 |BNSO US|BNSO                          |Equity      |                 |ElMeasIn|Industrial     |Electronics    |Electronic Mea                  |Common Stock|Common Stock |102517       |Bonso Electronics Internationa|
CA11031D1015|0       |16      |BBG000BPQN75|CA11031|11031101|N.A.     |BRTRF US|BRTRF  |Equity              |            |N.A.             |N.A.    |N.A.           |N.A.                                            |Common Stock  |Common Stock     |856048      |Bristol Trading Co Ltd|
61750K208|0|16|BBG111111111|US61750K2087|61750K208|B1FSFQ6|BPV-WA CN|BPV-WA|Equity|N.A.|N.A.|N.A.|N.A.|N.A.|Equity WRT|Warrant|17008561|N.A.|
000449421|10|16| | | | | | | | | | | | | | | | |
002168234|10|16| | | | | | | | | | | | | | | | |
973891WD8|10|16| | | | | | | | | | | | | | | | |
CA1084041203|0|16| |CA1084041203|108404120|B57T242|BPV-WA CN|BPV-WA|Equity|N.A.|N.A.|N.A.|N.A.|N.A.|Equity WRT|Warrant|17008561|N.A.|
CA10844D1006|10|16| | | | | | | | | | | | | | | | |
CA1089071064|0|16|BBG000BQF798|CA1089071064|108907106|2526032|BSV CN|BSV|Equity| |Quarrying|Basic Materials|Mining|Quarrying|Common Stock|Common Stock|110085|Bright Star Ventures Ltd|
CA10921R2046|0|16|BBG000G2B3F2|CA10921R2046|10921R204|B07C0M0|8289558Q CN|8289558Q|Equity| |Educational Software|Technology|Software|Educational Software|Common Stock|Common Stock|9601612|Brighter Minds Media Inc|
CA1094901107|0|16|BBG000V5R192|CA1094901107|109490110|B3Z68W1|BRD-W CN|BRD-W|Equity|11/19/2014|N.A.|N.A.|N.A.|N.A.|Equity WRT|Warrant|7787489|BRIGUS GOLD CORP|
CA1094905082|10|16| | | | | | | | | | | | | | | | |
CA109490AB88|10|16| | | | | | | | | | | | | | | | |
CA10973M1032|0|16|BBG000C3ZZ85|CA10973M1032|10973M103|2520755|370694Q CN|370694Q|Equity| |Capital Pools|Financial|Investment Companies|Capital Pools|Common Stock|Common Stock|883715|Brisbane Capital Corp|
CA11031D1015|0|16|BBG000BPQN75|CA11031D1015|11031D101|N.A.|BRTRF US|BRTRF|Equity| |N.A.|N.A.|N.A.|N.A.|Common Stock|Common Stock|856048|Bristol Trading Co Ltd|
VG0985293088|0|16|BBG000F640B2|VG0985293088|098529308|2110510|BNSO US|BNSO|Equity| |Electronic Measur Instr|Industrial|Electronics|Electronic Measur Instr|Common Stock|Common Stock|102517|Bonso Electronics Internationa|
VG2506391011|0|16|BBG000KCB2P6|VG2506391011|250639101|2265238|DSWL US|DSWL|Equity| |Diversified Manufact Op|Industrial|Miscellaneous Manufactur|Diversified Manufact Op|Common Stock|Common Stock|171129|Deswell Industries Inc|
VGG041361004|0|16|BBG000J2XGP5|VGG041361004|N.A.|B0JCH38|APWR US|APWR|Equity| |Power Conv/Supply Equip|Industrial|Electrical Compo&Equip|Power Conv/Supply Equip|Common Stock|Common Stock|9884197|A-Power Energy Generation Syst|
AU3CB0189975|0|16|BBG002N2JQK5|AU3CB0189975|EJ0177158|B4M3SP5| |NAB 6 02/15/17|Corp|02/15/2017|BANK|Financial|Banks|Commer Banks Non-US|AUSTRALIAN| |101034|NATIONAL AUSTRALIA BANK|
US3128S1L849|0|16|BBG00225MJ87|US3128S1L849|3128S1L84| | |FG T60351|Mtge|09/01/2041| |Mortgage Securities|FGLMC Collateral|FGLMC Other|MBS Other|Pool|701984|Freddie Mac|
US3128UG4W47|0|16|BBG00204JV61|US3128UG4W47|3128UG4W4| | |FH 1B8586|Mtge|08/01/2041| |Mortgage Securities|FHLMC Collateral|FHLMC ARM|MBS ARM|Pool|701983|Freddie Mac|
US3128UGXQ59|0|16|BBG001S2CT14|US3128UGXQ59|3128UGXQ5| | |FH 1B8436|Mtge|07/01/2041| |Mortgage Securities|FHLMC Collateral|FHLMC ARM|MBS ARM|Pool|701983|Freddie Mac|
END-OF-DATA
TIMEFINISHED=Wed Feb 25 14:28:29 EST 2015

END-OF-FILE
", ReqFooter = @"END-OF-DATA
END-OF-FILE
";
    #endregion
  }
}
