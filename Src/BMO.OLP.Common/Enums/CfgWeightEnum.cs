
namespace BMO.OLP.Common.Enums
{
	public enum CfgWeightEnum
	{
		DecimalFlag = 3,
		Binary = 4,
		Custom1 = 6,
		Equilateral = 7,
		New = 8,
		Percentage = 9,
		Custom3 = 10,
		Newer = 11
	}

	public enum FoundBy // synch with LKU_SEC_FETCH_STATE
	{
		NewIssuer = 0,
		SEDOL_TEC = 1,
		SEDOL = 2,
		TEC = 3,
		CUSIP = 4,
		ISIN = 5,
		PendingBbgFetch = 7,
		BbgFetch = 8,
		NotFound = 9,
		BbgEntityNotFound = 10,
		AltIssueNotFound = 11,
		FbExistgAiNewAe = 12,
		FbExistgAiAe = 13,
		NewEntityMap = 14,
		User = 15,
		MatchAlgo = 16,
	}
}
