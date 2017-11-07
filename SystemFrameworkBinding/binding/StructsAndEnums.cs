using System;
using ObjCRuntime;

namespace CryptoTokenKit
{
	[Native]
	public enum TKErrorCode : long
	{
		CodeNotImplemented = -1,
		CodeCommunicationError = -2,
		CodeCorruptedData = -3,
		CodeCanceledByUser = -4,
		CodeAuthenticationFailed = -5,
		CodeObjectNotFound = -6,
		CodeTokenNotFound = -7,
		CodeBadParameter = -8,
		CodeAuthenticationNeeded = -9,
		AuthenticationFailed = CodeAuthenticationFailed,
		ObjectNotFound = CodeObjectNotFound,
		TokenNotFound = CodeTokenNotFound
	}

	[Native]
	public enum TKSmartCardProtocol : long
	{
		None = 0,
		T0 = (1 << 0),
		T1 = (1 << 1),
		T15 = (1 << 15),
		Any = (1 << 16) - 1
	}

	[Native]
	public enum TKSmartCardSlotState : long
	{
		Missing = 0,
		Empty = 1,
		Probing = 2,
		MuteCard = 3,
		ValidCard = 4
	}

	[Native]
	public enum TKSmartCardPINCharset : long
	{
		Numeric = 0,
		Alphanumeric = 1,
		UpperAlphanumeric = 2
	}

	[Native]
	public enum TKSmartCardPINEncoding : long
	{
		Binary = 0,
		Ascii = 1,
		Bcd = 2
	}

	[Native]
	public enum TKSmartCardPINJustification : long
	{
		Left = 0,
		Right = 1
	}

	[Native]
	public enum TKSmartCardPINCompletion : long
	{
		MaxLength = (1 << 0),
		Key = (1 << 1),
		Timeout = (1 << 2)
	}

	[Native]
	public enum TKSmartCardPINConfirmation : long
	{
		None = 0,
		New = (1 << 0),
		Current = (1 << 1)
	}

	[Native]
	public enum TKTokenOperation : long
	{
		None = 0,
		ReadData = 1,
		SignData = 2,
		DecryptData = 3,
		PerformKeyExchange = 4
	}
}