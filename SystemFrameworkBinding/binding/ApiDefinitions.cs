using System;
using CryptoTokenKit;
using Foundation;
using ObjCRuntime;
using Security;

namespace CryptoTokenKit
{
	[Static]
	partial interface Constants
	{
		[Field ("TKErrorDomain", "/System/Library/Frameworks/CryptoTokenKit.framework/CryptoTokenKit")]
		NSString TKErrorDomain { get; }
	}

	// @interface TKTLVRecord : NSObject
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TKTLVRecord
	{
		// @property (readonly, nonatomic) TKTLVTag tag;
		[Export ("tag")]
		ulong Tag { get; }

		// @property (readonly, nonatomic) NSData * _Nonnull value;
		[Export ("value")]
		NSData Value { get; }

		// @property (readonly, nonatomic) NSData * _Nonnull data;
		[Export ("data")]
		NSData Data { get; }

		// +(instancetype _Nullable)recordFromData:(NSData * _Nonnull)data;
		[Static]
		[Export ("recordFromData:")]
		[return: NullAllowed]
		TKTLVRecord RecordFromData (NSData data);

		// +(NSArray<TKTLVRecord *> * _Nullable)sequenceOfRecordsFromData:(NSData * _Nonnull)data;
		[Static]
		[Export ("sequenceOfRecordsFromData:")]
		[return: NullAllowed]
		TKTLVRecord [] SequenceOfRecordsFromData (NSData data);
	}

	// @interface TKBERTLVRecord : TKTLVRecord
	[BaseType (typeof (TKTLVRecord))]
	interface TKBERTLVRecord
	{
		// +(NSData * _Nonnull)dataForTag:(TKTLVTag)tag;
		[Static]
		[Export ("dataForTag:")]
		NSData DataForTag (ulong tag);

		// -(instancetype _Nonnull)initWithTag:(TKTLVTag)tag value:(NSData * _Nonnull)value;
		[Export ("initWithTag:value:")]
		IntPtr Constructor (ulong tag, NSData value);

		// -(instancetype _Nonnull)initWithTag:(TKTLVTag)tag records:(NSArray<TKTLVRecord *> * _Nonnull)records;
		[Export ("initWithTag:records:")]
		IntPtr Constructor (ulong tag, TKTLVRecord [] records);
	}

	// @interface TKSimpleTLVRecord : TKTLVRecord
	[BaseType (typeof (TKTLVRecord))]
	interface TKSimpleTLVRecord
	{
		// -(instancetype _Nonnull)initWithTag:(UInt8)tag value:(NSData * _Nonnull)value;
		[Export ("initWithTag:value:")]
		IntPtr Constructor (byte tag, NSData value);
	}

	// @interface TKCompactTLVRecord : TKTLVRecord
	[BaseType (typeof (TKTLVRecord))]
	interface TKCompactTLVRecord
	{
		// -(instancetype _Nonnull)initWithTag:(UInt8)tag value:(NSData * _Nonnull)value;
		[Export ("initWithTag:value:")]
		IntPtr Constructor (byte tag, NSData value);
	}

	// @interface TKSmartCardATRInterfaceGroup : NSObject
	[BaseType (typeof (NSObject))]
	interface TKSmartCardATRInterfaceGroup
	{
		// @property (readonly, nonatomic) NSNumber * _Nullable TA;
		[NullAllowed, Export ("TA")]
		NSNumber TA { get; }

		// @property (readonly, nonatomic) NSNumber * _Nullable TB;
		[NullAllowed, Export ("TB")]
		NSNumber TB { get; }

		// @property (readonly, nonatomic) NSNumber * _Nullable TC;
		[NullAllowed, Export ("TC")]
		NSNumber TC { get; }

		// @property (readonly, nonatomic) NSNumber * _Nullable protocol;
		[NullAllowed, Export ("protocol")]
		NSNumber Protocol { get; }
	}

	// @interface TKSmartCardATR : NSObject
	[BaseType (typeof (NSObject))]
	interface TKSmartCardATR
	{
		// -(instancetype _Nullable)initWithBytes:(NSData * _Nonnull)bytes;
		[Export ("initWithBytes:")]
		IntPtr Constructor (NSData bytes);

		// -(instancetype _Nullable)initWithSource:(int (^ _Nonnull)())source;
		[Export ("initWithSource:")]
		IntPtr Constructor (Func<int> source);

		// @property (readonly, nonatomic) NSData * _Nonnull bytes;
		[Export ("bytes")]
		NSData Bytes { get; }

		// @property (readonly, nonatomic) NSArray<NSNumber *> * _Nonnull protocols;
		[Export ("protocols")]
		NSNumber [] Protocols { get; }

		// -(TKSmartCardATRInterfaceGroup * _Nullable)interfaceGroupAtIndex:(NSInteger)index;
		[Export ("interfaceGroupAtIndex:")]
		[return: NullAllowed]
		TKSmartCardATRInterfaceGroup InterfaceGroupAtIndex (nint index);

		// -(TKSmartCardATRInterfaceGroup * _Nullable)interfaceGroupForProtocol:(TKSmartCardProtocol)protocol;
		[Export ("interfaceGroupForProtocol:")]
		[return: NullAllowed]
		TKSmartCardATRInterfaceGroup InterfaceGroupForProtocol (TKSmartCardProtocol protocol);

		// @property (readonly, nonatomic) NSData * _Nonnull historicalBytes;
		[Export ("historicalBytes")]
		NSData HistoricalBytes { get; }

		// @property (readonly, nonatomic) NSArray<TKCompactTLVRecord *> * _Nullable historicalRecords __attribute__((availability(watchos, introduced=4.0))) __attribute__((availability(tvos, introduced=11.0))) __attribute__((availability(ios, introduced=10.0))) __attribute__((availability(macos, introduced=10.12)));
		[NullAllowed, Export ("historicalRecords")]
		TKCompactTLVRecord [] HistoricalRecords { get; }
	}

	// @interface TKSmartCardSlotManager : NSObject
	[BaseType (typeof (NSObject))]
	interface TKSmartCardSlotManager
	{
		// @property (readonly, class) TKSmartCardSlotManager * _Nullable defaultManager;
		[Static]
		[NullAllowed, Export ("defaultManager")]
		TKSmartCardSlotManager DefaultManager { get; }

		// @property (readonly) NSArray<NSString *> * _Nonnull slotNames;
		[Export ("slotNames")]
		string [] SlotNames { get; }

		// -(void)getSlotWithName:(NSString * _Nonnull)name reply:(void (^ _Nonnull)(TKSmartCardSlot * _Nullable))reply;
		[Export ("getSlotWithName:reply:")]
		void GetSlotWithName (string name, Action<TKSmartCardSlot> reply);

		// -(TKSmartCardSlot * _Nullable)slotNamed:(NSString * _Nonnull)name __attribute__((availability(macos, introduced=10.13)));
		[Export ("slotNamed:")]
		[return: NullAllowed]
		TKSmartCardSlot SlotNamed (string name);
	}

	// @interface TKSmartCardPINFormat : NSObject
	[BaseType (typeof (NSObject))]
	interface TKSmartCardPINFormat
	{
		// @property TKSmartCardPINCharset charset;
		[Export ("charset", ArgumentSemantic.Assign)]
		TKSmartCardPINCharset Charset { get; set; }

		// @property TKSmartCardPINEncoding encoding;
		[Export ("encoding", ArgumentSemantic.Assign)]
		TKSmartCardPINEncoding Encoding { get; set; }

		// @property NSInteger minPINLength;
		[Export ("minPINLength")]
		nint MinPINLength { get; set; }

		// @property NSInteger maxPINLength;
		[Export ("maxPINLength")]
		nint MaxPINLength { get; set; }

		// @property NSInteger PINBlockByteLength;
		[Export ("PINBlockByteLength")]
		nint PINBlockByteLength { get; set; }

		// @property TKSmartCardPINJustification PINJustification;
		[Export ("PINJustification", ArgumentSemantic.Assign)]
		TKSmartCardPINJustification PINJustification { get; set; }

		// @property NSInteger PINBitOffset;
		[Export ("PINBitOffset")]
		nint PINBitOffset { get; set; }

		// @property NSInteger PINLengthBitOffset;
		[Export ("PINLengthBitOffset")]
		nint PINLengthBitOffset { get; set; }

		// @property NSInteger PINLengthBitSize;
		[Export ("PINLengthBitSize")]
		nint PINLengthBitSize { get; set; }
	}

	class ITKSmartCardUserInteractionDelegate {}

	// @protocol TKSmartCardUserInteractionDelegate
	[Protocol, Model]
	interface TKSmartCardUserInteractionDelegate
	{
		// @optional -(void)characterEnteredInUserInteraction:(TKSmartCardUserInteraction * _Nonnull)interaction;
		[Export ("characterEnteredInUserInteraction:")]
		void CharacterEnteredInUserInteraction (TKSmartCardUserInteraction interaction);

		// @optional -(void)correctionKeyPressedInUserInteraction:(TKSmartCardUserInteraction * _Nonnull)interaction;
		[Export ("correctionKeyPressedInUserInteraction:")]
		void CorrectionKeyPressedInUserInteraction (TKSmartCardUserInteraction interaction);

		// @optional -(void)validationKeyPressedInUserInteraction:(TKSmartCardUserInteraction * _Nonnull)interaction;
		[Export ("validationKeyPressedInUserInteraction:")]
		void ValidationKeyPressedInUserInteraction (TKSmartCardUserInteraction interaction);

		// @optional -(void)invalidCharacterEnteredInUserInteraction:(TKSmartCardUserInteraction * _Nonnull)interaction;
		[Export ("invalidCharacterEnteredInUserInteraction:")]
		void InvalidCharacterEnteredInUserInteraction (TKSmartCardUserInteraction interaction);

		// @optional -(void)oldPINRequestedInUserInteraction:(TKSmartCardUserInteraction * _Nonnull)interaction;
		[Export ("oldPINRequestedInUserInteraction:")]
		void OldPINRequestedInUserInteraction (TKSmartCardUserInteraction interaction);

		// @optional -(void)newPINRequestedInUserInteraction:(TKSmartCardUserInteraction * _Nonnull)interaction;
		[Export ("newPINRequestedInUserInteraction:")]
		void NewPINRequestedInUserInteraction (TKSmartCardUserInteraction interaction);

		// @optional -(void)newPINConfirmationRequestedInUserInteraction:(TKSmartCardUserInteraction * _Nonnull)interaction;
		[Export ("newPINConfirmationRequestedInUserInteraction:")]
		void NewPINConfirmationRequestedInUserInteraction (TKSmartCardUserInteraction interaction);
	}

	// @interface TKSmartCardUserInteraction : NSObject
	[BaseType (typeof (NSObject))]
	interface TKSmartCardUserInteraction
	{
		[Protocolize]
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		TKSmartCardUserInteractionDelegate Delegate { get; set; }

		// @property (weak) id<TKSmartCardUserInteractionDelegate> _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		// @property NSTimeInterval initialTimeout;
		[Export ("initialTimeout")]
		double InitialTimeout { get; set; }

		// @property NSTimeInterval interactionTimeout;
		[Export ("interactionTimeout")]
		double InteractionTimeout { get; set; }

		// -(void)runWithReply:(void (^ _Nonnull)(BOOL, NSError * _Nullable))reply;
		[Export ("runWithReply:")]
		void RunWithReply (Action<bool, NSError> reply);

		// -(BOOL)cancel;
		[Export ("cancel")]
		bool Cancel ();
	}

	// @interface TKSmartCardUserInteractionForPINOperation : TKSmartCardUserInteraction
	[BaseType (typeof (TKSmartCardUserInteraction))]
	interface TKSmartCardUserInteractionForPINOperation
	{
		// @property TKSmartCardPINCompletion PINCompletion;
		[Export ("PINCompletion", ArgumentSemantic.Assign)]
		TKSmartCardPINCompletion PINCompletion { get; set; }

		// @property NSArray<NSNumber *> * _Nullable PINMessageIndices;
		[NullAllowed, Export ("PINMessageIndices", ArgumentSemantic.Assign)]
		NSNumber [] PINMessageIndices { get; set; }

		// @property NSLocale * _Null_unspecified locale;
		[Export ("locale", ArgumentSemantic.Assign)]
		NSLocale Locale { get; set; }

		// @property UInt16 resultSW;
		[Export ("resultSW")]
		ushort ResultSW { get; set; }

		// @property NSData * _Nullable resultData;
		[NullAllowed, Export ("resultData", ArgumentSemantic.Assign)]
		NSData ResultData { get; set; }
	}

	// @interface TKSmartCardUserInteractionForSecurePINVerification : TKSmartCardUserInteractionForPINOperation
	[BaseType (typeof (TKSmartCardUserInteractionForPINOperation))]
	interface TKSmartCardUserInteractionForSecurePINVerification
	{
	}

	// @interface TKSmartCardUserInteractionForSecurePINChange : TKSmartCardUserInteractionForPINOperation
	[BaseType (typeof (TKSmartCardUserInteractionForPINOperation))]
	interface TKSmartCardUserInteractionForSecurePINChange
	{
		// @property TKSmartCardPINConfirmation PINConfirmation;
		[Export ("PINConfirmation", ArgumentSemantic.Assign)]
		TKSmartCardPINConfirmation PINConfirmation { get; set; }
	}

	// @interface TKSmartCardSlot : NSObject
	[BaseType (typeof (NSObject))]
	interface TKSmartCardSlot
	{
		// @property (readonly) TKSmartCardSlotState state;
		[Export ("state")]
		TKSmartCardSlotState State { get; }

		// @property (readonly) TKSmartCardATR * _Nullable ATR;
		[NullAllowed, Export ("ATR")]
		TKSmartCardATR ATR { get; }

		// @property (readonly, nonatomic) NSString * _Nonnull name;
		[Export ("name")]
		string Name { get; }

		// @property (readonly, nonatomic) NSInteger maxInputLength;
		[Export ("maxInputLength")]
		nint MaxInputLength { get; }

		// @property (readonly, nonatomic) NSInteger maxOutputLength;
		[Export ("maxOutputLength")]
		nint MaxOutputLength { get; }

		// -(TKSmartCard * _Nullable)makeSmartCard;
		[NullAllowed, Export ("makeSmartCard")]
		TKSmartCard MakeSmartCard { get; }
	}

	// @interface TKSmartCard : NSObject
	[BaseType (typeof (NSObject))]
	interface TKSmartCard
	{
		// @property (readonly, nonatomic) TKSmartCardSlot * _Nonnull slot;
		[Export ("slot")]
		TKSmartCardSlot Slot { get; }

		// @property (readonly) BOOL valid;
		[Export ("valid")]
		bool Valid { get; }

		// @property TKSmartCardProtocol allowedProtocols;
		[Export ("allowedProtocols", ArgumentSemantic.Assign)]
		TKSmartCardProtocol AllowedProtocols { get; set; }

		// @property (readonly) TKSmartCardProtocol currentProtocol;
		[Export ("currentProtocol")]
		TKSmartCardProtocol CurrentProtocol { get; }

		// @property BOOL sensitive;
		[Export ("sensitive")]
		bool Sensitive { get; set; }

		// @property id _Nullable context;
		[NullAllowed, Export ("context", ArgumentSemantic.Assign)]
		NSObject Context { get; set; }

		// -(void)beginSessionWithReply:(void (^ _Nonnull)(BOOL, NSError * _Nullable))reply;
		[Export ("beginSessionWithReply:")]
		void BeginSessionWithReply (Action<bool, NSError> reply);

		// -(void)transmitRequest:(NSData * _Nonnull)request reply:(void (^ _Nonnull)(NSData * _Nullable, NSError * _Nullable))reply;
		[Export ("transmitRequest:reply:")]
		void TransmitRequest (NSData request, Action<NSData, NSError> reply);

		// -(void)endSession;
		[Export ("endSession")]
		void EndSession ();

		// -(TKSmartCardUserInteractionForSecurePINVerification * _Nullable)userInteractionForSecurePINVerificationWithPINFormat:(TKSmartCardPINFormat * _Nonnull)PINFormat APDU:(NSData * _Nonnull)APDU PINByteOffset:(NSInteger)PINByteOffset __attribute__((availability(macos, introduced=10.11)));
		[Export ("userInteractionForSecurePINVerificationWithPINFormat:APDU:PINByteOffset:")]
		[return: NullAllowed]
		TKSmartCardUserInteractionForSecurePINVerification UserInteractionForSecurePINVerificationWithPINFormat (TKSmartCardPINFormat PINFormat, NSData APDU, nint PINByteOffset);

		// -(TKSmartCardUserInteractionForSecurePINChange * _Nullable)userInteractionForSecurePINChangeWithPINFormat:(TKSmartCardPINFormat * _Nonnull)PINFormat APDU:(NSData * _Nonnull)APDU currentPINByteOffset:(NSInteger)currentPINByteOffset newPINByteOffset:(NSInteger)newPINByteOffset __attribute__((availability(macos, introduced=10.11)));
		[Export ("userInteractionForSecurePINChangeWithPINFormat:APDU:currentPINByteOffset:newPINByteOffset:")]
		[return: NullAllowed]
		TKSmartCardUserInteractionForSecurePINChange UserInteractionForSecurePINChangeWithPINFormat (TKSmartCardPINFormat PINFormat, NSData APDU, nint currentPINByteOffset, nint newPINByteOffset);
	}

	// @interface APDULevelTransmit (TKSmartCard)
	//[Category]
	//[BaseType (typeof (TKSmartCard))]
	//interface TKSmartCard_APDULevelTransmit
	//{
	//	// @property UInt8 cla __attribute__((availability(watchos, introduced=4.0))) __attribute__((availability(tvos, introduced=11.0))) __attribute__((availability(ios, introduced=9.0))) __attribute__((availability(macos, introduced=10.10)));
	//	[Export ("cla")]
	//	byte Cla { get; set; }

	//	// @property BOOL useExtendedLength __attribute__((availability(watchos, introduced=4.0))) __attribute__((availability(tvos, introduced=11.0))) __attribute__((availability(ios, introduced=9.0))) __attribute__((availability(macos, introduced=10.10)));
	//	[Export ("useExtendedLength")]
	//	bool UseExtendedLength { get; set; }

	//	// @property BOOL useCommandChaining __attribute__((availability(watchos, introduced=4.0))) __attribute__((availability(tvos, introduced=11.0))) __attribute__((availability(ios, introduced=10.0))) __attribute__((availability(macos, introduced=10.12)));
	//	[Export ("useCommandChaining")]
	//	bool UseCommandChaining { get; set; }

	//	// -(void)sendIns:(UInt8)ins p1:(UInt8)p1 p2:(UInt8)p2 data:(NSData * _Nullable)requestData le:(NSNumber * _Nullable)le reply:(void (^ _Nonnull)(NSData * _Nullable, UInt16, NSError * _Nullable))reply __attribute__((availability(watchos, introduced=4.0))) __attribute__((availability(tvos, introduced=11.0))) __attribute__((availability(ios, introduced=9.0))) __attribute__((availability(macos, introduced=10.10)));
	//	[Export ("sendIns:p1:p2:data:le:reply:")]
	//	void SendIns (byte ins, byte p1, byte p2, [NullAllowed] NSData requestData, [NullAllowed] NSNumber le, Action<NSData, ushort, NSError> reply);

	//	// -(BOOL)inSessionWithError:(NSError * _Nullable * _Nullable)error executeBlock:(BOOL (^ _Nonnull)(NSError * _Nullable * _Nullable))block __attribute__((availability(watchos, introduced=4.0))) __attribute__((availability(tvos, introduced=11.0))) __attribute__((availability(ios, introduced=10.0))) __attribute__((availability(macos, introduced=10.12)));
	//	[Watch (4, 0), TV (11, 0), Mac (10, 12), iOS (10, 0)]
	//	[Export ("inSessionWithError:executeBlock:")]
	//	unsafe bool InSessionWithError ([NullAllowed] out NSError error, Func<Foundation.NSError*, bool> block);

	//	 -(NSData * _Nullable)sendIns:(UInt8)ins p1:(UInt8)p1 p2:(UInt8)p2 data:(NSData * _Nullable)requestData le:(NSNumber * _Nullable)le sw:(UInt16 * _Nonnull)sw error:(NSError * _Nullable * _Nullable)error __attribute__((availability(watchos, introduced=4.0))) __attribute__((availability(tvos, introduced=11.0))) __attribute__((availability(ios, introduced=10.0))) __attribute__((availability(macos, introduced=10.12)));
	//	[Export ("sendIns:p1:p2:data:le:sw:error:")]
	//	[return: NullAllowed]
	//	unsafe NSData SendIns (byte ins, byte p1, byte p2, [NullAllowed] NSData requestData, [NullAllowed] NSNumber le, ushort* sw, [NullAllowed] out NSError error);
	//}

	// @interface TKTokenKeyAlgorithm : NSObject
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TKTokenKeyAlgorithm
	{
		// -(BOOL)isAlgorithm:(SecKeyAlgorithm _Nonnull)algorithm;
		[Export ("isAlgorithm:")]
		unsafe bool IsAlgorithm (IntPtr algorithm);

		// -(BOOL)supportsAlgorithm:(SecKeyAlgorithm _Nonnull)algorithm;
		[Export ("supportsAlgorithm:")]
		unsafe bool SupportsAlgorithm (IntPtr algorithm);
	}

	// @interface TKTokenKeyExchangeParameters : NSObject
	[BaseType (typeof (NSObject))]
	interface TKTokenKeyExchangeParameters
	{
		// @property (readonly) NSInteger requestedSize;
		[Export ("requestedSize")]
		nint RequestedSize { get; }

		// @property (readonly, copy) NSData * _Nullable sharedInfo;
		[NullAllowed, Export ("sharedInfo", ArgumentSemantic.Copy)]
		NSData SharedInfo { get; }
	}

	// @interface TKTokenSession : NSObject
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TKTokenSession
	{
		// -(instancetype _Nonnull)initWithToken:(TKToken * _Nonnull)token __attribute__((objc_designated_initializer));
		[Export ("initWithToken:")]
		[DesignatedInitializer]
		IntPtr Constructor (TKToken token);

		// @property (readonly) TKToken * _Nonnull token;
		[Export ("token")]
		TKToken Token { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		TKTokenSessionDelegate Delegate { get; set; }

		// @property (weak) id<TKTokenSessionDelegate> _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	// @protocol TKTokenSessionDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface TKTokenSessionDelegate
	{
		// @optional -(TKTokenAuthOperation * _Nullable)tokenSession:(TKTokenSession * _Nonnull)session beginAuthForOperation:(TKTokenOperation)operation constraint:(TKTokenOperationConstraint _Nonnull)constraint error:(NSError * _Nullable * _Nullable)error;
		[Export ("tokenSession:beginAuthForOperation:constraint:error:")]
		[return: NullAllowed]
		TKTokenAuthOperation BeginAuth (TKTokenSession session, TKTokenOperation operation, NSObject constraint, [NullAllowed] out NSError error);

		// @optional -(BOOL)tokenSession:(TKTokenSession * _Nonnull)session supportsOperation:(TKTokenOperation)operation usingKey:(TKTokenObjectID _Nonnull)keyObjectID algorithm:(TKTokenKeyAlgorithm * _Nonnull)algorithm;
		[Export ("tokenSession:supportsOperation:usingKey:algorithm:")]
		bool SupportsOperation (TKTokenSession session, TKTokenOperation operation, NSObject keyObjectID, TKTokenKeyAlgorithm algorithm);

		// @optional -(NSData * _Nullable)tokenSession:(TKTokenSession * _Nonnull)session signData:(NSData * _Nonnull)dataToSign usingKey:(TKTokenObjectID _Nonnull)keyObjectID algorithm:(TKTokenKeyAlgorithm * _Nonnull)algorithm error:(NSError * _Nullable * _Nullable)error;
		[Export ("tokenSession:signData:usingKey:algorithm:error:")]
		[return: NullAllowed]
		NSData SignData (TKTokenSession session, NSData dataToSign, NSObject keyObjectID, TKTokenKeyAlgorithm algorithm, [NullAllowed] out NSError error);

		// @optional -(NSData * _Nullable)tokenSession:(TKTokenSession * _Nonnull)session decryptData:(NSData * _Nonnull)ciphertext usingKey:(TKTokenObjectID _Nonnull)keyObjectID algorithm:(TKTokenKeyAlgorithm * _Nonnull)algorithm error:(NSError * _Nullable * _Nullable)error;
		[Export ("tokenSession:decryptData:usingKey:algorithm:error:")]
		[return: NullAllowed]
		NSData DecryptData (TKTokenSession session, NSData ciphertext, NSObject keyObjectID, TKTokenKeyAlgorithm algorithm, [NullAllowed] out NSError error);

		// @optional -(NSData * _Nullable)tokenSession:(TKTokenSession * _Nonnull)session performKeyExchangeWithPublicKey:(NSData * _Nonnull)otherPartyPublicKeyData usingKey:(TKTokenObjectID _Nonnull)objectID algorithm:(TKTokenKeyAlgorithm * _Nonnull)algorithm parameters:(TKTokenKeyExchangeParameters * _Nonnull)parameters error:(NSError * _Nullable * _Nullable)error;
		[Export ("tokenSession:performKeyExchangeWithPublicKey:usingKey:algorithm:parameters:error:")]
		[return: NullAllowed]
		NSData PerformKeyExchangeWithPublicKey (TKTokenSession session, NSData otherPartyPublicKeyData, NSObject objectID, TKTokenKeyAlgorithm algorithm, TKTokenKeyExchangeParameters parameters, [NullAllowed] out NSError error);
	}

	// @interface TKToken : NSObject
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TKToken
	{
		// -(instancetype _Nonnull)initWithTokenDriver:(TKTokenDriver * _Nonnull)tokenDriver instanceID:(NSString * _Nonnull)instanceID __attribute__((objc_designated_initializer));
		[Export ("initWithTokenDriver:instanceID:")]
		[DesignatedInitializer]
		IntPtr Constructor (TKTokenDriver tokenDriver, string instanceID);

		// @property (readonly) TKTokenDriver * _Nonnull tokenDriver;
		[Export ("tokenDriver")]
		TKTokenDriver TokenDriver { get; }

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		TKTokenDelegate Delegate { get; set; }

		// @property (weak) id<TKTokenDelegate> _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		// @property (readonly) TKTokenKeychainContents * _Nullable keychainContents;
		[NullAllowed, Export ("keychainContents")]
		TKTokenKeychainContents KeychainContents { get; }
	}

	// @protocol TKTokenDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface TKTokenDelegate
	{
		// @required -(TKTokenSession * _Nullable)token:(TKToken * _Nonnull)token createSessionWithError:(NSError * _Nullable * _Nullable)error;
		[Abstract]
		[Export ("token:createSessionWithError:")]
		[return: NullAllowed]
		TKTokenSession Token (TKToken token, [NullAllowed] out NSError error);

		// @optional -(void)token:(TKToken * _Nonnull)token terminateSession:(TKTokenSession * _Nonnull)session;
		[Export ("token:terminateSession:")]
		void Token (TKToken token, TKTokenSession session);
	}

	// @interface TKTokenDriver : NSObject
	[BaseType (typeof (NSObject))]
	interface TKTokenDriver
	{
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		TKTokenDriverDelegate Delegate { get; set; }

		// @property (weak) id<TKTokenDriverDelegate> _Nullable delegate;
		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }
	}

	// @protocol TKTokenDriverDelegate <NSObject>
	[Protocol, Model]
	[BaseType (typeof (NSObject))]
	interface TKTokenDriverDelegate
	{
		// @optional -(void)tokenDriver:(TKTokenDriver * _Nonnull)driver terminateToken:(TKToken * _Nonnull)token;
		[Export ("tokenDriver:terminateToken:")]
		void TerminateToken (TKTokenDriver driver, TKToken token);
	}

	// @interface TKTokenAuthOperation : NSObject <NSSecureCoding>
	[BaseType (typeof (NSObject))]
	interface TKTokenAuthOperation : INSSecureCoding
	{
		// -(BOOL)finishWithError:(NSError * _Nullable * _Nullable)error;
		[Export ("finishWithError:")]
		bool FinishWithError ([NullAllowed] out NSError error);
	}

	// @interface TKTokenPasswordAuthOperation : TKTokenAuthOperation
	[BaseType (typeof (TKTokenAuthOperation))]
	interface TKTokenPasswordAuthOperation
	{
		// @property (copy) NSString * _Nullable password;
		[NullAllowed, Export ("password")]
		string Password { get; set; }
	}

	// @interface TKTokenKeychainItem : NSObject
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TKTokenKeychainItem
	{
		// -(instancetype _Nonnull)initWithObjectID:(TKTokenObjectID _Nonnull)objectID __attribute__((objc_designated_initializer));
		[Export ("initWithObjectID:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSObject objectID);

		// @property (readonly, copy) TKTokenObjectID _Nonnull objectID;
		[Export ("objectID", ArgumentSemantic.Copy)]
		NSObject ObjectID { get; }

		// @property (copy) NSString * _Nullable label;
		[NullAllowed, Export ("label")]
		string Label { get; set; }

		// @property (copy) NSDictionary<NSNumber *,TKTokenOperationConstraint> * _Nullable constraints;
		[NullAllowed, Export ("constraints", ArgumentSemantic.Copy)]
		NSDictionary<NSNumber, NSObject> Constraints { get; set; }
	}

	// @interface TKTokenKeychainCertificate : TKTokenKeychainItem
	[BaseType (typeof (TKTokenKeychainItem))]
	interface TKTokenKeychainCertificate
	{
		// -(instancetype _Nullable)initWithCertificate:(SecCertificateRef _Nonnull)certificateRef objectID:(TKTokenObjectID _Nonnull)objectID __attribute__((objc_designated_initializer));
		[Export ("initWithCertificate:objectID:")]
		[DesignatedInitializer]
		unsafe IntPtr Constructor (IntPtr certificateRef, NSObject objectID);

		// @property (readonly, copy) NSData * _Nonnull data;
		[Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; }
	}

	// @interface TKTokenKeychainKey : TKTokenKeychainItem
	[BaseType (typeof (TKTokenKeychainItem))]
	interface TKTokenKeychainKey
	{
		// -(instancetype _Nullable)initWithCertificate:(SecCertificateRef _Nullable)certificateRef objectID:(TKTokenObjectID _Nonnull)objectID __attribute__((objc_designated_initializer));
		[Export ("initWithCertificate:objectID:")]
		[DesignatedInitializer]
		unsafe IntPtr Constructor ([NullAllowed] IntPtr certificateRef, NSObject objectID);

		// @property (copy) NSString * _Nonnull keyType;
		[Export ("keyType")]
		string KeyType { get; set; }

		// @property (copy) NSData * _Nullable applicationTag;
		[NullAllowed, Export ("applicationTag", ArgumentSemantic.Copy)]
		NSData ApplicationTag { get; set; }

		// @property NSInteger keySizeInBits;
		[Export ("keySizeInBits")]
		nint KeySizeInBits { get; set; }

		// @property (copy) NSData * _Nullable publicKeyData;
		[NullAllowed, Export ("publicKeyData", ArgumentSemantic.Copy)]
		NSData PublicKeyData { get; set; }

		// @property (copy) NSData * _Nullable publicKeyHash;
		[NullAllowed, Export ("publicKeyHash", ArgumentSemantic.Copy)]
		NSData PublicKeyHash { get; set; }

		// @property BOOL canDecrypt;
		[Export ("canDecrypt")]
		bool CanDecrypt { get; set; }

		// @property BOOL canSign;
		[Export ("canSign")]
		bool CanSign { get; set; }

		// @property BOOL canPerformKeyExchange;
		[Export ("canPerformKeyExchange")]
		bool CanPerformKeyExchange { get; set; }

		// @property (getter = isSuitableForLogin) BOOL suitableForLogin;
		[Export ("suitableForLogin")]
		bool SuitableForLogin { [Bind ("isSuitableForLogin")] get; set; }
	}

	// @interface TKTokenKeychainContents : NSObject
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface TKTokenKeychainContents
	{
		// -(void)fillWithItems:(NSArray<TKTokenKeychainItem *> * _Nonnull)items;
		[Export ("fillWithItems:")]
		void FillWithItems (TKTokenKeychainItem [] items);

		// @property (readonly, copy) NSArray<TKTokenKeychainItem *> * _Nonnull items;
		[Export ("items", ArgumentSemantic.Copy)]
		TKTokenKeychainItem [] Items { get; }

		// -(TKTokenKeychainKey * _Nullable)keyForObjectID:(TKTokenObjectID _Nonnull)objectID error:(NSError * _Nullable * _Nullable)error;
		[Export ("keyForObjectID:error:")]
		[return: NullAllowed]
		TKTokenKeychainKey KeyForObjectID (NSObject objectID, [NullAllowed] out NSError error);

		// -(TKTokenKeychainCertificate * _Nullable)certificateForObjectID:(TKTokenObjectID _Nonnull)objectID error:(NSError * _Nullable * _Nullable)error;
		[Export ("certificateForObjectID:error:")]
		[return: NullAllowed]
		TKTokenKeychainCertificate CertificateForObjectID (NSObject objectID, [NullAllowed] out NSError error);
	}

	// @interface TKTokenSmartCardPINAuthOperation : TKTokenAuthOperation
	[BaseType (typeof (TKTokenAuthOperation))]
	interface TKTokenSmartCardPINAuthOperation
	{
		// @property TKSmartCardPINFormat * _Nonnull PINFormat;
		[Export ("PINFormat", ArgumentSemantic.Assign)]
		TKSmartCardPINFormat PINFormat { get; set; }

		// @property (copy) NSData * _Nullable APDUTemplate;
		[NullAllowed, Export ("APDUTemplate", ArgumentSemantic.Copy)]
		NSData APDUTemplate { get; set; }

		// @property NSInteger PINByteOffset;
		[Export ("PINByteOffset")]
		nint PINByteOffset { get; set; }

		// @property TKSmartCard * _Nullable smartCard;
		[NullAllowed, Export ("smartCard", ArgumentSemantic.Assign)]
		TKSmartCard SmartCard { get; set; }

		// @property (copy) NSString * _Nullable PIN;
		[NullAllowed, Export ("PIN")]
		string PIN { get; set; }
	}

	// @interface TKSmartCardTokenSession : TKTokenSession
	[BaseType (typeof (TKTokenSession))]
	interface TKSmartCardTokenSession
	{
		// @property (readonly) TKSmartCard * _Nonnull smartCard;
		[Export ("smartCard")]
		TKSmartCard SmartCard { get; }
	}

	// @interface TKSmartCardToken : TKToken
	[BaseType (typeof (TKToken))]
	interface TKSmartCardToken
	{
		// -(instancetype _Nonnull)initWithSmartCard:(TKSmartCard * _Nonnull)smartCard AID:(NSData * _Nullable)AID instanceID:(NSString * _Nonnull)instanceID tokenDriver:(TKSmartCardTokenDriver * _Nonnull)tokenDriver __attribute__((objc_designated_initializer));
		[Export ("initWithSmartCard:AID:instanceID:tokenDriver:")]
		[DesignatedInitializer]
		IntPtr Constructor (TKSmartCard smartCard, [NullAllowed] NSData AID, string instanceID, TKSmartCardTokenDriver tokenDriver);

		// @property (readonly) NSData * _Nullable AID;
		[NullAllowed, Export ("AID")]
		NSData AID { get; }
	}

	// @interface TKSmartCardTokenDriver : TKTokenDriver
	[BaseType (typeof (TKTokenDriver))]
	interface TKSmartCardTokenDriver
	{
	}

	// @protocol TKSmartCardTokenDriverDelegate <TKTokenDriverDelegate>
	//[Protocol, Model]
	//interface TKSmartCardTokenDriverDelegate : ITKTokenDriverDelegate
	//{
	//	// @required -(TKSmartCardToken * _Nullable)tokenDriver:(TKSmartCardTokenDriver * _Nonnull)driver createTokenForSmartCard:(TKSmartCard * _Nonnull)smartCard AID:(NSData * _Nullable)AID error:(NSError * _Nullable * _Nullable)error;
	//	[Abstract]
	//	[Export ("tokenDriver:createTokenForSmartCard:AID:error:")]
	//	[return: NullAllowed]
	//	TKSmartCardToken CreateTokenForSmartCard (TKSmartCardTokenDriver driver, TKSmartCard smartCard, [NullAllowed] NSData AID, [NullAllowed] out NSError error);
	//}

	// @interface TKTokenWatcher : NSObject
	[BaseType (typeof (NSObject))]
	interface TKTokenWatcher
	{
		// @property (readonly) NSArray<NSString *> * _Nonnull tokenIDs;
		[Export ("tokenIDs")]
		string [] TokenIDs { get; }

		// -(instancetype _Nonnull)initWithInsertionHandler:(void (^ _Nonnull)(NSString * _Nonnull))insertionHandler __attribute__((availability(ios, deprecated=10.13))) __attribute__((availability(ios, introduced=10.12))) __attribute__((availability(macos, deprecated=10.13))) __attribute__((availability(macos, introduced=10.12)));
		[Deprecated (PlatformName.iOS, 10, 13, message: "Use 'setInsertionHandler' instead")]
		[Deprecated (PlatformName.MacOSX, 10, 13, message: "Use 'setInsertionHandler' instead")]
		[Export ("initWithInsertionHandler:")]
		IntPtr Constructor (Action<NSString> insertionHandler);

		// -(void)setInsertionHandler:(void (^ _Nonnull)(NSString * _Nonnull))insertionHandler __attribute__((availability(ios, introduced=10.13))) __attribute__((availability(macos, introduced=10.13)));
		[Export ("setInsertionHandler:")]
		void SetInsertionHandler (Action<NSString> insertionHandler);

		// -(void)addRemovalHandler:(void (^ _Nonnull)(NSString * _Nonnull))removalHandler forTokenID:(NSString * _Nonnull)tokenID;
		[Export ("addRemovalHandler:forTokenID:")]
		void AddRemovalHandler (Action<NSString> removalHandler, string tokenID);
	}
}