// This file contains api definitions shared between AppKit and UIKit

using System;
using System.Diagnostics;
using System.ComponentModel;
using Foundation;
using ObjCRuntime;
#if !WATCH
using CoreAnimation;
#endif
using CoreGraphics;

using CGGlyph=System.UInt16;
using NSGlyph=System.UInt32;

#if !MONOMAC
using NSColor=UIKit.UIColor;
using NSFont=UIKit.UIFont;
#endif

// dummy types to simplify build
#if !MONOMAC
using NSCell=System.Object;
using NSGlyphGenerator=System.Object;
using NSGlyphStorageOptions=System.Object;
using NSImageScaling=System.Object;
using NSRulerMarker=System.Object;
using NSRulerView=System.Object;
using NSTextAttachmentCell=System.Object;
using NSTextBlock=System.Object;
using NSTextList=System.Object;
using NSTextTableBlock=System.Object;
using NSTextTabType=System.Object;
using NSTextStorageEditedFlags=System.Object;
using NSTextView=System.Object;
using NSTypesetter=System.Object;
using NSTypesetterBehavior=System.Object;
using NSView=System.Object;
using NSWindow=System.Object;
#if WATCH
using CATransform3D=System.Object;
using NSTextContainer=System.Object;
using NSTextStorage=System.Object;
using UIDynamicItem=System.Object;
using UITraitCollection = Foundation.NSObject;
#endif // WATCH
#else
using UICollectionLayoutListConfiguration=System.Object;
using UIContentInsetsReference=System.Object;
using UITraitCollection=System.Object;
#endif // !MONOMAC

#if MONOMAC
using BezierPath=AppKit.NSBezierPath;
using Image=AppKit.NSImage;
using TextAlignment=AppKit.NSTextAlignment;
using LineBreakMode=AppKit.NSLineBreakMode;
using CollectionLayoutSectionOrthogonalScrollingBehavior=AppKit.NSCollectionLayoutSectionOrthogonalScrollingBehavior;
using CollectionElementCategory=AppKit.NSCollectionElementCategory;
using StringAttributes=AppKit.NSStringAttributes;
using View=AppKit.NSView;
#else
using BezierPath=UIKit.UIBezierPath;
using Image=UIKit.UIImage;
using TextAlignment=UIKit.UITextAlignment;
using LineBreakMode=UIKit.UILineBreakMode;
using CollectionLayoutSectionOrthogonalScrollingBehavior=UIKit.UICollectionLayoutSectionOrthogonalScrollingBehavior;
using CollectionElementCategory=UIKit.UICollectionElementCategory;
using StringAttributes=UIKit.UIStringAttributes;
#if WATCH
using View=System.Object;
#else
using View=UIKit.UIView;
#endif
#endif

#if MONOMAC
namespace AppKit {
#else
namespace UIKit {
#endif

#if XAMCORE_4_0 || MONOMAC
	delegate void NSTextLayoutEnumerateLineFragments (CGRect rect, CGRect usedRectangle, NSTextContainer textContainer, NSRange glyphRange, out bool stop);
	delegate void NSTextLayoutEnumerateEnclosingRects (CGRect rect, out bool stop);
#else
	delegate void NSTextLayoutEnumerateLineFragments (CGRect rect, CGRect usedRectangle, NSTextContainer textContainer, NSRange glyphRange, ref bool stop);
	delegate void NSTextLayoutEnumerateEnclosingRects (CGRect rect, ref bool stop);
#endif

	// NSInteger -> NSLayoutManager.h
	[Native]
	[Flags]
	[NoWatch]
	[Mac (10,11)]
	[MacCatalyst (13,0)]
	public enum NSControlCharacterAction : long {
		ZeroAdvancement = (1 << 0),
		Whitespace = (1 << 1),
		HorizontalTab = (1 << 2),
		LineBreak = (1 << 3),
		ParagraphBreak = (1 << 4),
		ContainerBreak = (1 << 5),

#if !XAMCORE_4_0 && !__MACCATALYST__ && !MONOMAC
		[Obsolete ("Use 'ZeroAdvancement' instead.")]
		ZeroAdvancementAction = ZeroAdvancement,
		[Obsolete ("Use 'Whitespace' instead.")]
		WhitespaceAction = Whitespace,
		[Obsolete ("Use 'HorizontalTab' instead.")]
		HorizontalTabAction = HorizontalTab,
		[Obsolete ("Use 'LineBreak' instead.")]
		LineBreakAction = LineBreak,
		[Obsolete ("Use 'ParagraphBreak' instead.")]
		ParagraphBreakAction = ParagraphBreak,
		[Obsolete ("Use 'ContainerBreak' instead.")]
		ContainerBreakAction = ContainerBreak,
#endif
	}

	[Mac (10,15), Watch (6,0), TV (13,0), iOS (13,0), MacCatalyst (13,0)]
	[Flags]
	[Native]
	public enum NSDirectionalRectEdge : ulong
	{
		None = 0x0,
		Top = 1uL << 0,
		Leading = 1uL << 1,
		Bottom = 1uL << 2,
		Trailing = 1uL << 3,
		All = Top | Leading | Bottom | Trailing,
	}

	// NSInteger -> NSLayoutManager.h
	[NoWatch]
	[Mac (10,11)]
	[MacCatalyst (13,0)]
	[Native]
	public enum NSGlyphProperty : long {
		Null = (1 << 0),
		ControlCharacter = (1 << 1),
		Elastic = (1 << 2),
		NonBaseCharacter = (1 << 3),
	}

	// NSInteger -> NSLayoutConstraint.h
	[Native]
	[NoWatch]
	[MacCatalyst (13,0)]
	public enum NSLayoutAttribute : long {
		NoAttribute = 0,
		Left = 1,
		Right,
		Top,
		Bottom,
		Leading,
		Trailing,
		Width,
		Height,
		CenterX,
		CenterY,
		Baseline,
		[Mac (10,11)]
		LastBaseline = Baseline,
		[Mac (10,11)]
		FirstBaseline,

		[NoMac]
		[iOS (8,0)]
		LeftMargin,
		[NoMac]
		[iOS (8,0)]
		RightMargin,
		[NoMac]
		[iOS (8,0)]
		TopMargin,
		[NoMac]
		[iOS (8,0)]
		BottomMargin,
		[NoMac]
		[iOS (8,0)]
		LeadingMargin,
		[NoMac]
		[iOS (8,0)]
		TrailingMargin,
		[NoMac]
		[iOS (8,0)]
		CenterXWithinMargins,
		[NoMac]
		[iOS (8,0)]
		CenterYWithinMargins,
	}

	// NSUInteger -> NSLayoutConstraint.h
	[Native]
	[Flags]
	[NoWatch]
	[MacCatalyst (13,0)]
	public enum NSLayoutFormatOptions : ulong {
		None = 0,

		AlignAllLeft = (1 << (int) NSLayoutAttribute.Left),
		AlignAllRight = (1 << (int) NSLayoutAttribute.Right),
		AlignAllTop = (1 << (int) NSLayoutAttribute.Top),
		AlignAllBottom = (1 << (int) NSLayoutAttribute.Bottom),
		AlignAllLeading = (1 << (int) NSLayoutAttribute.Leading),
		AlignAllTrailing = (1 << (int) NSLayoutAttribute.Trailing),
		AlignAllCenterX = (1 << (int) NSLayoutAttribute.CenterX),
		AlignAllCenterY = (1 << (int) NSLayoutAttribute.CenterY),
		AlignAllBaseline = (1 << (int) NSLayoutAttribute.Baseline),
		[Mac (10,11)]
		AlignAllLastBaseline = (1 << (int) NSLayoutAttribute.LastBaseline),
		[Mac (10,11)]
		AlignAllFirstBaseline = (1 << (int) NSLayoutAttribute.FirstBaseline),

		AlignmentMask = 0xFFFF,

		/* choose only one of these three
		 */
		DirectionLeadingToTrailing = 0 << 16, // default
		DirectionLeftToRight = 1 << 16,
		DirectionRightToLeft = 2 << 16,

		[NoMac]
		SpacingEdgeToEdge = 0 << 19,
		[NoMac]
		SpacingBaselineToBaseline = 1 << 19,
		[NoMac]
		SpacingMask = 1 << 19,

		DirectionMask = 0x3 << 16,
	}

	// NSInteger -> UITextInput.h
	[Native]
	[NoWatch]
	[MacCatalyst (13,0)]
	public enum NSLayoutRelation : long {
		LessThanOrEqual = -1,
		Equal = 0,
		GreaterThanOrEqual = 1,
	}

	[Watch (7,0), TV (14,0), iOS (14,0)]
	[Mac (11,0)]
	[MacCatalyst (13,0)]
	[Flags]
	[Native]
	public enum NSLineBreakStrategy : ulong {
		None = 0x0,
		PushOut = 1uL << 0,
		HangulWordPriority = 1uL << 1,
		Standard = 0xffff,
	}

	[Watch (6,0), TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[MacCatalyst (13,0)]
	[Native]
	public enum NSRectAlignment : long
	{
		None = 0,
		Top,
		TopLeading,
		Leading,
		BottomLeading,
		Bottom,
		BottomTrailing,
		Trailing,
		TopTrailing,
	}

	[Mac (10,15), iOS (13,0), TV (13,0)]
	[MacCatalyst (13,0)]
	[Native]
	public enum NSTextScalingType : long
	{
		Standard = 0,
		iOS,
	}

	// NSInteger -> NSLayoutManager.h
	[Native]
	[NoWatch]
	[MacCatalyst (13,0)]
	public enum NSTextLayoutOrientation : long {
		Horizontal,
		Vertical,
	}

	// NSUInteger -> NSTextStorage.h
	[Mac (10,11)]
	[Native]
	[Flags]
	[NoWatch]
	public enum NSTextStorageEditActions : ulong {
		Attributes = 1,
		Characters = 2,
	}

	[NoWatch] // Header is not present in watchOS SDK.
	[iOS (7,0)]
	[DesignatedDefaultCtor]
	[BaseType (typeof (NSObject))]
	partial interface NSLayoutManager : NSSecureCoding {

#if !XAMCORE_4_0
		// This was removed in the headers in the macOS 10.11 SDK
		[NoiOS][NoTV]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'TextStorage' instead.")]
		[Export ("attributedString")]
		NSAttributedString AttributedString { get; }
#endif

		[Export ("textContainers")]
		NSTextContainer [] TextContainers { get; }

		[Export ("addTextContainer:")]
		void AddTextContainer (NSTextContainer container);

		[Export ("insertTextContainer:atIndex:")]
		void InsertTextContainer (NSTextContainer container, /* NSUInteger */ nint index);

		[Export ("removeTextContainerAtIndex:")]
		void RemoveTextContainer (/* NSUInteger */ nint index);

		[Export ("textContainerChangedGeometry:")]
		void TextContainerChangedGeometry (NSTextContainer container);

		[NoiOS][NoTV]
		[Export ("textContainerChangedTextView:")]
		void TextContainerChangedTextView (NSTextContainer container);

#if !XAMCORE_4_0
		// This was removed in the headers in the macOS 10.11 SDK
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11)]
		[Export ("layoutOptions")]
		NSGlyphStorageOptions LayoutOptions { get; }
#endif

		[Export ("hasNonContiguousLayout")]
		bool HasNonContiguousLayout { get; }

		/* InvalidateGlyphs */
#if XAMCORE_4_0 || MONOMAC
		[Protected]
#else
		[Internal][Sealed]
#endif
		[Export ("invalidateGlyphsForCharacterRange:changeInLength:actualCharacterRange:")]
		void InvalidateGlyphs (NSRange characterRange, /* NSInteger */ nint delta, /* nullable NSRangePointer */ IntPtr actualCharacterRange);

		[Wrap ("InvalidateGlyphs (characterRange, delta, IntPtr.Zero)")]
		void InvalidateGlyphs (NSRange characterRange, /* NSInteger */ nint delta);

#if XAMCORE_4_0 || MONOMAC
		[Sealed]
#endif
		[Export ("invalidateGlyphsForCharacterRange:changeInLength:actualCharacterRange:")]
#if XAMCORE_4_0 || MONOMAC
		void InvalidateGlyphs (NSRange characterRange, /* NSInteger */ nint delta, /* nullable NSRangePointer */ out NSRange actualCharacterRange);
#else
		void InvalidateGlyphs (NSRange charRange, /* NSInteger */ nint delta, /* nullable NSRangePointer */ out NSRange actualCharRange);
#endif

		/* InvalidateLayout */
#if XAMCORE_4_0 || MONOMAC
		[Protected]
#else
		[Internal][Sealed]
#endif
		[Export ("invalidateLayoutForCharacterRange:actualCharacterRange:")]
		void InvalidateLayout (NSRange characterRange, /* nullable NSRangePointer */ IntPtr actualCharacterRange);

		[Wrap ("InvalidateLayout (characterRange, IntPtr.Zero)")]
		void InvalidateLayout (NSRange characterRange);

#if XAMCORE_4_0 || MONOMAC
		[Sealed]
#endif
		[Export ("invalidateLayoutForCharacterRange:actualCharacterRange:")]
#if XAMCORE_4_0 || MONOMAC
		void InvalidateLayout (NSRange characterRange, /* nullable NSRangePointer */ out NSRange actualCharacterRange);
#else
		void InvalidateLayout (NSRange charRange, /* nullable NSRangePointer */ out NSRange actualCharRange);
#endif

		[Export ("invalidateDisplayForCharacterRange:")]
#if XAMCORE_4_0
		void InvalidateDisplayForCharacterRange (NSRange characterRange);
#else
		void InvalidateDisplayForCharacterRange (NSRange charRange);
#endif

		[Export ("invalidateDisplayForGlyphRange:")]
		void InvalidateDisplayForGlyphRange (NSRange glyphRange);

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use ProcessEditing (NSTextStorage textStorage, NSTextStorageEditActions editMask, NSRange newCharacterRange, nint delta, NSRange invalidatedCharacterRange) instead).")]
		[Export ("textStorage:edited:range:changeInLength:invalidatedRange:")]
		void TextStorageEdited (NSTextStorage str, NSTextStorageEditedFlags editedMask, NSRange newCharRange, nint changeInLength, NSRange invalidatedCharRange);
#endif

		[Export ("ensureGlyphsForCharacterRange:")]
#if XAMCORE_4_0
		void EnsureGlyphsForCharacterRange (NSRange characterRange);
#else
		void EnsureGlyphsForCharacterRange (NSRange charRange);
#endif

		[Export ("ensureGlyphsForGlyphRange:")]
		void EnsureGlyphsForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForCharacterRange:")]
#if XAMCORE_4_0
		void EnsureLayoutForCharacterRange (NSRange characterRange);
#else
		void EnsureLayoutForCharacterRange (NSRange charRange);
#endif

		[Export ("ensureLayoutForGlyphRange:")]
		void EnsureLayoutForGlyphRange (NSRange glyphRange);

		[Export ("ensureLayoutForTextContainer:")]
		void EnsureLayoutForTextContainer (NSTextContainer container);

		[Export ("ensureLayoutForBoundingRect:inTextContainer:")]
		void EnsureLayoutForBoundingRect (CGRect bounds, NSTextContainer container);

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'SetGlyphs' instead.")]
		[Export ("insertGlyph:atGlyphIndex:characterIndex:")]
		void InsertGlyph (NSGlyph glyph, nint glyphIndex, nint charIndex);
#endif

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'SetGlyphs' instead.")]
		[Export ("replaceGlyphAtIndex:withGlyph:")]
		void ReplaceGlyphAtIndex (nint glyphIndex, NSGlyph newGlyph);
#endif

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'SetGlyphs' instead.")]
		[Export ("deleteGlyphsInRange:")]
		void DeleteGlyphs (NSRange glyphRange);
#endif

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'SetGlyphs' instead.")]
		[Export ("setCharacterIndex:forGlyphAtIndex:")]
		void SetCharacterIndex (nint charIndex, nint glyphIndex);
#endif

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'SetGlyphs' instead.")]
		[Export ("setIntAttribute:value:forGlyphAtIndex:")]
		void SetIntAttribute (nint attributeTag, nint value, nint glyphIndex);
#endif

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'SetGlyphs' instead.")]
		[Export ("invalidateGlyphsOnLayoutInvalidationForGlyphRange:")]
		void InvalidateGlyphsOnLayoutInvalidation (NSRange glyphRange);
#endif

		[Export ("numberOfGlyphs")]
#if XAMCORE_4_0 || !MONOMAC
		/* NSUInteger */ nuint NumberOfGlyphs { get; }
#else
		/* NSUInteger */ nint NumberOfGlyphs { get; }
#endif

#if !XAMCORE_4_0
		[Export ("glyphAtIndex:isValidIndex:")]
#if MONOMAC
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'GetCGGlyph' instead).")]
		NSGlyph GlyphAtIndex (nint glyphIndex, ref bool isValidIndex);
#else
 		[Availability (Deprecated = Platform.iOS_9_0, Message = "Use 'GetGlyph' instead.")]
		CGGlyph GlyphAtIndex (nuint glyphIndex, ref bool isValidIndex);
#endif // MONOMAC
#endif // !XAMCORE_4_0

#if !XAMCORE_4_0
		[Export ("glyphAtIndex:")]
#if MONOMAC
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'GetCGGlyph' instead).")]
		NSGlyph GlyphAtIndex (nint glyphIndex);
#else
 		[Availability (Deprecated = Platform.iOS_9_0, Message = "Use 'GetGlyph' instead.")]
		CGGlyph GlyphAtIndex (nuint glyphIndex);
#endif // MONOMAC
#endif // !XAMCORE_4_0

		[Export ("isValidGlyphIndex:")]
#if XAMCORE_4_0
		bool IsValidGlyph (nuint glyphIndex);
#elif MONOMAC
		bool IsValidGlyphIndex (nint glyphIndex);
#else
		bool IsValidGlyphIndex (nuint glyphIndex);
#endif

		[Export ("characterIndexForGlyphAtIndex:")]
#if XAMCORE_4_0
		nuint GetCharacterIndex (nuint glyphIndex);
#elif MONOMAC
		nuint CharacterIndexForGlyphAtIndex (nint glyphIndex);
#else
		nuint CharacterIndexForGlyphAtIndex (nuint glyphIndex);
#endif

		[Export ("glyphIndexForCharacterAtIndex:")]
#if XAMCORE_4_0
		nuint GetGlyphIndex (nuint characterIndex);
#elif MONOMAC
		nuint GlyphIndexForCharacterAtIndex (nint charIndex);
#else
		nuint GlyphIndexForCharacterAtIndex (nuint charIndex);
#endif

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use 'GetGlyphs' instead).")]
		[Export ("intAttribute:forGlyphAtIndex:")]
		nint GetIntAttribute (nint attributeTag, nint glyphIndex);
#endif

		[Export ("setTextContainer:forGlyphRange:")]
#if XAMCORE_4_0 || !MONOMAC
		void SetTextContainer (NSTextContainer container, NSRange glyphRange);
#else
		void SetTextContainerForRange (NSTextContainer container, NSRange glyphRange);
#endif

		[Export ("setLineFragmentRect:forGlyphRange:usedRect:")]
#if XAMCORE_4_0
		void SetLineFragment (CGRect fragmentRect, NSRange glyphRange, CGRect usedRect);
#else
		void SetLineFragmentRect (CGRect fragmentRect, NSRange glyphRange, CGRect usedRect);
#endif

		[Export ("setExtraLineFragmentRect:usedRect:textContainer:")]
#if XAMCORE_4_0
		void SetExtraLineFragment (CGRect fragmentRect, CGRect usedRect, NSTextContainer container);
#else
		void SetExtraLineFragmentRect (CGRect fragmentRect, CGRect usedRect, NSTextContainer container);
#endif

		[Export ("setLocation:forStartOfGlyphRange:")]
#if MONOMAC || XAMCORE_4_0
		void SetLocation (CGPoint location, NSRange forStartOfGlyphRange);
#else
		void SetLocation (CGPoint location, NSRange glyphRange);
#endif

		[Export ("setNotShownAttribute:forGlyphAtIndex:")]
#if XAMCORE_4_0 || !MONOMAC
		void SetNotShownAttribute (bool flag, nuint glyphIndex);
#else
		void SetNotShownAttribute (bool flag, nint glyphIndex);
#endif

		[Export ("setDrawsOutsideLineFragment:forGlyphAtIndex:")]
#if XAMCORE_4_0 || !MONOMAC
		void SetDrawsOutsideLineFragment (bool flag, nuint glyphIndex);
#else
		void SetDrawsOutsideLineFragment (bool flag, nint glyphIndex);
#endif

		[Export ("setAttachmentSize:forGlyphRange:")]
		void SetAttachmentSize (CGSize attachmentSize, NSRange glyphRange);

		[Export ("getFirstUnlaidCharacterIndex:glyphIndex:")]
#if XAMCORE_4_0
		void GetFirstUnlaid (out nuint characterIndex, out nuint glyphIndex);
#else
		void GetFirstUnlaidCharacterIndex (ref nuint charIndex, ref nuint glyphIndex);
#endif

		[Export ("firstUnlaidCharacterIndex")]
#if XAMCORE_4_0 || !MONOMAC
		nuint FirstUnlaidCharacterIndex { get; }
#else
		nint FirstUnlaidCharacterIndex { get; }
#endif

		[Export ("firstUnlaidGlyphIndex")]
#if XAMCORE_4_0 || !MONOMAC
		nuint FirstUnlaidGlyphIndex { get; }
#else
		nint FirstUnlaidGlyphIndex { get; }
#endif

		/* GetTextContainer */
#if XAMCORE_4_0 || MONOMAC
		[Protected]
#else
		[Sealed][Internal]
#endif
		[Export ("textContainerForGlyphAtIndex:effectiveRange:")]
		NSTextContainer GetTextContainer (nuint glyphIndex, /* nullable NSRangePointer */ IntPtr effectiveGlyphRange);

		[Wrap ("GetTextContainer (glyphIndex, IntPtr.Zero)")]
		NSTextContainer GetTextContainer (nuint glyphIndex);

#if XAMCORE_4_0 || MONOMAC
		[Sealed]
#endif
		[Export ("textContainerForGlyphAtIndex:effectiveRange:")]
		NSTextContainer GetTextContainer (nuint glyphIndex, /* nullable NSRangePointer */ out NSRange effectiveGlyphRange);

#if XAMCORE_4_0 || MONOMAC
		[Protected]
#else
		[Sealed][Internal]
#endif
		[Export ("textContainerForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		NSTextContainer GetTextContainer (nuint glyphIndex, IntPtr effectiveGlyphRange, bool withoutAdditionalLayout);

		[Wrap ("GetTextContainer (glyphIndex, IntPtr.Zero, flag)")]
		NSTextContainer GetTextContainer (nuint glyphIndex, bool flag);

#if XAMCORE_4_0 || MONOMAC
		[Sealed]
#endif
		[Export ("textContainerForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		NSTextContainer GetTextContainer (nuint glyphIndex, /* nullable NSRangePointer */ out NSRange effectiveGlyphRange, bool withoutAdditionalLayout);

		[Export ("usedRectForTextContainer:")]
#if XAMCORE_4_0
		CGRect GetUsedRect (NSTextContainer container);
#else
		CGRect GetUsedRectForTextContainer (NSTextContainer container);
#endif

		/* GetLineFragmentRect (NSUInteger, NSRangePointer) */
		[Protected]
		[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:")]
		CGRect GetLineFragmentRect (nuint glyphIndex, /* nullable NSRangePointer */ IntPtr effectiveGlyphRange);

		[Wrap ("GetLineFragmentRect (glyphIndex, IntPtr.Zero)")]
		CGRect GetLineFragmentRect (nuint glyphIndex);

		[Sealed]
		[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:")]
		CGRect GetLineFragmentRect (nuint glyphIndex, out /* nullable NSRangePointer */ NSRange effectiveGlyphRange);

		/* GetLineFragmentRect (NSUInteger, NSRangePointer, bool) */
		[iOS (9,0)]
#if MONOMAC || XAMCORE_4_0
		[Protected]
#else
		[Sealed][Internal]
#endif
		[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		CGRect GetLineFragmentRect (nuint glyphIndex, /* nullable NSRangePointer */ IntPtr effectiveGlyphRange, bool withoutAdditionalLayout);

		[iOS (9,0)]
		[Wrap ("GetLineFragmentRect (glyphIndex, IntPtr.Zero)")]
		CGRect GetLineFragmentRect (nuint glyphIndex, bool withoutAdditionalLayout);

		[iOS (9,0)]
#if MONOMAC || XAMCORE_4_0
		[Sealed]
#endif
		[Export ("lineFragmentRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		CGRect GetLineFragmentRect (nuint glyphIndex, out /* nullable NSRangePointer */ NSRange effectiveGlyphRange, bool withoutAdditionalLayout);

		/* GetLineFragmentUsedRect (NSUInteger, NSRangePointer) */
		[Protected]
		[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:")]
		CGRect GetLineFragmentUsedRect (nuint glyphIndex, /* nullable NSRangePointer */ IntPtr effectiveGlyphRange);

		[Wrap ("GetLineFragmentUsedRect (glyphIndex, IntPtr.Zero)")]
		CGRect GetLineFragmentUsedRect (nuint glyphIndex);

		[Sealed]
		[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:")]
		CGRect GetLineFragmentUsedRect (nuint glyphIndex, out /* nullable NSRangePointer */ NSRange effectiveGlyphRange);

		/* GetLineFragmentUsedRect (NSUInteger, NSRangePointer, bool) */
		[iOS (9,0)]
#if MONOMAC || XAMCORE_4_0
		[Protected]
#else
		[Sealed][Internal]
#endif
		[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		CGRect GetLineFragmentUsedRect (nuint glyphIndex, /* nullable NSRangePointer */ IntPtr effectiveGlyphRange, bool withoutAdditionalLayout);

		[iOS (9,0)]
		[Wrap ("GetLineFragmentUsedRect (glyphIndex, IntPtr.Zero)")]
		CGRect GetLineFragmentUsedRect (nuint glyphIndex, bool withoutAdditionalLayout);

		[iOS (9,0)]
#if MONOMAC || XAMCORE_4_0
		[Sealed]
#endif
		[Export ("lineFragmentUsedRectForGlyphAtIndex:effectiveRange:withoutAdditionalLayout:")]
		CGRect GetLineFragmentUsedRect (nuint glyphIndex, out /* nullable NSRangePointer */ NSRange effectiveGlyphRange, bool withoutAdditionalLayout);

		[Export ("extraLineFragmentRect")]
		CGRect ExtraLineFragmentRect { get; }

		[Export ("extraLineFragmentUsedRect")]
		CGRect ExtraLineFragmentUsedRect { get; }

		[Export ("extraLineFragmentTextContainer")]
		NSTextContainer ExtraLineFragmentTextContainer { get; }

		[Export ("locationForGlyphAtIndex:")]
#if XAMCORE_4_0
		CGPoint GetLocationForGlyph (nuint glyphIndex);
#elif MONOMAC
		CGPoint LocationForGlyphAtIndex (nint glyphIndex);
#else
		CGPoint LocationForGlyphAtIndex (nuint glyphIndex);
#endif

		[Export ("notShownAttributeForGlyphAtIndex:")]
#if XAMCORE_4_0
		bool NotShownAttributeForGlyph (nuint glyphIndex);
#elif MONOMAC
		bool NotShownAttributeForGlyphAtIndex (nint glyphIndex);
#else
		bool NotShownAttributeForGlyphAtIndex (nuint glyphIndex);
#endif

		[Export ("drawsOutsideLineFragmentForGlyphAtIndex:")]
#if XAMCORE_4_0
		bool DrawsOutsideLineFragmentForGlyph (nuint glyphIndex);
#elif MONOMAC
		bool DrawsOutsideLineFragmentForGlyphAt (nint glyphIndex);
#else
		bool DrawsOutsideLineFragmentForGlyphAtIndex (nuint glyphIndex);
#endif

		[Export ("attachmentSizeForGlyphAtIndex:")]
#if XAMCORE_4_0
		CGSize GetAttachmentSizeForGlyph (nuint glyphIndex);
#elif MONOMAC
		CGSize AttachmentSizeForGlyphAt (nint glyphIndex);
#else
		CGSize AttachmentSizeForGlyphAtIndex (nuint glyphIndex);
#endif

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("setLayoutRect:forTextBlock:glyphRange:")]
		void SetLayoutRect (CGRect layoutRect, NSTextBlock forTextBlock, NSRange glyphRange);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("setBoundsRect:forTextBlock:glyphRange:")]
		void SetBoundsRect (CGRect boundsRect, NSTextBlock forTextBlock, NSRange glyphRange);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("layoutRectForTextBlock:glyphRange:")]
#if XAMCORE_4_0
		CGRect GetLayoutRect (NSTextBlock block, NSRange glyphRange);
#else
		CGRect LayoutRect (NSTextBlock block, NSRange glyphRange);
#endif

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("boundsRectForTextBlock:glyphRange:")]
#if XAMCORE_4_0
		CGRect GetBoundsRect (NSTextBlock block, NSRange glyphRange);
#else
		CGRect BoundsRect (NSTextBlock block, NSRange glyphRange);
#endif

		/* GetLayoutRect (NSTextBlock, NSUInteger, nullable NSRangePointer) */

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Protected]
		[Export ("layoutRectForTextBlock:atIndex:effectiveRange:")]
		CGRect GetLayoutRect (NSTextBlock block, nuint glyphIndex, IntPtr effectiveGlyphRange);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Wrap ("GetLayoutRect (block, glyphIndex, IntPtr.Zero)")]
		CGRect GetLayoutRect (NSTextBlock block, nuint glyphIndex);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Sealed]
		[Export ("layoutRectForTextBlock:atIndex:effectiveRange:")]
		CGRect GetLayoutRect (NSTextBlock block, nuint glyphIndex, out NSRange effectiveGlyphRange);

		/* GetBoundsRect (NSTextBlock, NSUInteger, nullable NSRangePointer) */

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Protected]
		[Export ("boundsRectForTextBlock:atIndex:effectiveRange:")]
		CGRect GetBoundsRect (NSTextBlock block, nuint glyphIndex, IntPtr effectiveGlyphRange);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Wrap ("GetBoundsRect (block, glyphIndex, IntPtr.Zero)")]
		CGRect GetBoundsRect (NSTextBlock block, nuint glyphIndex);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Sealed]
		[Export ("boundsRectForTextBlock:atIndex:effectiveRange:")]
		CGRect GetBoundsRect (NSTextBlock block, nuint glyphIndex, out NSRange effectiveGlyphRange);

		/* GetGlyphRange (NSRange, nullable NSRangePointer) */

#if XAMCORE_4_0 || !MONOMAC
		[Protected]
#else
		[Internal][Sealed]
#endif
		[Export ("glyphRangeForCharacterRange:actualCharacterRange:")]
		NSRange GetGlyphRange (NSRange characterRange, IntPtr actualCharacterRange);

		[Wrap ("GetGlyphRange (characterRange, IntPtr.Zero)")]
		NSRange GetGlyphRange (NSRange characterRange);

		[Sealed]
		[Export ("glyphRangeForCharacterRange:actualCharacterRange:")]
		NSRange GetGlyphRange (NSRange characterRange, out NSRange actualCharacterRange);

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Obsolete ("Use 'GetGlyphRange' instead.")]
		[Export ("glyphRangeForCharacterRange:actualCharacterRange:")]
		NSRange GlyphRangeForCharacterRange (NSRange charRange, out NSRange actualCharRange);
#endif

		/* GetCharacterRange (NSRange, nullable NSRangePointer) */
#if XAMCORE_4_0 || !MONOMAC
		[Protected]
#else
		[Internal][Sealed]
#endif
		[Export ("characterRangeForGlyphRange:actualGlyphRange:")]
		NSRange GetCharacterRange (NSRange glyphRange, IntPtr actualGlyphRange);

		[Wrap ("GetCharacterRange (glyphRange, IntPtr.Zero)")]
		NSRange GetCharacterRange (NSRange glyphRange);

		[Sealed]
		[Export ("characterRangeForGlyphRange:actualGlyphRange:")]
		NSRange GetCharacterRange (NSRange glyphRange, out NSRange actualGlyphRange);

#if MONOMAC && !XAMCORE_4_0
		[Obsolete ("Use 'GetCharacterRange' instead.")]
		[Export ("characterRangeForGlyphRange:actualGlyphRange:")]
		NSRange CharacterRangeForGlyphRange (NSRange glyphRange, out NSRange actualGlyphRange);
#endif

		[Export ("glyphRangeForTextContainer:")]
		NSRange GetGlyphRange (NSTextContainer container);

		[Export ("rangeOfNominallySpacedGlyphsContainingIndex:")]
#if XAMCORE_4_0
		NSRange GetRangeOfNominallySpacedGlyphsContainingIndex (nuint glyphIndex);
#elif MONOMAC
		NSRange RangeOfNominallySpacedGlyphsContainingIndex (nint glyphIndex);
#else
		NSRange RangeOfNominallySpacedGlyphsContainingIndex (nuint glyphIndex);
#endif

		[Internal]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("rectArrayForGlyphRange:withinSelectedGlyphRange:inTextContainer:rectCount:")]
		[Availability (Deprecated = Platform.Mac_10_11)]
		IntPtr GetRectArray (NSRange glyphRange, NSRange selectedGlyphRange, IntPtr textContainerHandle, out nuint rectCount);

		[Export ("boundingRectForGlyphRange:inTextContainer:")]
#if XAMCORE_4_0
		CGRect GetBoundingRect (NSRange glyphRange, NSTextContainer container);
#else
		CGRect BoundingRectForGlyphRange (NSRange glyphRange, NSTextContainer container);
#endif

		[Export ("glyphRangeForBoundingRect:inTextContainer:")]
#if XAMCORE_4_0
		NSRange GetGlyphRangeForBoundingRect (CGRect bounds, NSTextContainer container);
#else
		NSRange GlyphRangeForBoundingRect (CGRect bounds, NSTextContainer container);
#endif

		[Export ("glyphRangeForBoundingRectWithoutAdditionalLayout:inTextContainer:")]
#if XAMCORE_4_0
		NSRange GetGlyphRangeForBoundingRectWithoutAdditionalLayout (CGRect bounds, NSTextContainer container);
#else
		NSRange GlyphRangeForBoundingRectWithoutAdditionalLayout (CGRect bounds, NSTextContainer container);
#endif

		[Export ("glyphIndexForPoint:inTextContainer:fractionOfDistanceThroughGlyph:")]
#if XAMCORE_4_0
		nuint GetGlyphIndex (CGPoint point, NSTextContainer container, /* nullable CGFloat */ out nfloat fractionOfDistanceThroughGlyph);
#elif MONOMAC
		nuint GlyphIndexForPointInTextContainer (CGPoint point, NSTextContainer container, ref nfloat fractionOfDistanceThroughGlyph);
#else
		nuint GlyphIndexForPoint (CGPoint point, NSTextContainer container, ref nfloat partialFraction);
#endif

		[Export ("glyphIndexForPoint:inTextContainer:")]
#if XAMCORE_4_0
		nuint GetGlyphIndex (CGPoint point, NSTextContainer container);
#else
		nuint GlyphIndexForPoint (CGPoint point, NSTextContainer container);
#endif

		[Export ("fractionOfDistanceThroughGlyphForPoint:inTextContainer:")]
#if XAMCORE_4_0
		nfloat GetFractionOfDistanceThroughGlyph (CGPoint point, NSTextContainer container);
#else
		nfloat FractionOfDistanceThroughGlyphForPoint (CGPoint point, NSTextContainer container);
#endif

		// GetCharacterIndex (CGPoint, NSTextContainer, nullable CGFloat*)
#if XAMCORE_4_0
		[Protected]
#else
		[Sealed][Internal]
#endif
		[Export ("characterIndexForPoint:inTextContainer:fractionOfDistanceBetweenInsertionPoints:")]
		nuint GetCharacterIndex (CGPoint point, NSTextContainer container, IntPtr fractionOfDistanceBetweenInsertionPoints);

		[Wrap ("GetCharacterIndex (point, container, IntPtr.Zero)")]
		nuint GetCharacterIndex (CGPoint point, NSTextContainer container);

		[Sealed]
		[Export ("characterIndexForPoint:inTextContainer:fractionOfDistanceBetweenInsertionPoints:")]
		nuint GetCharacterIndex (CGPoint point, NSTextContainer container, out nfloat fractionOfDistanceBetweenInsertionPoints);

#if !XAMCORE_4_0
		[Obsolete ("Use 'GetCharacterIndex' instead.")]
		[Export ("characterIndexForPoint:inTextContainer:fractionOfDistanceBetweenInsertionPoints:")]
#if MONOMAC
		nuint CharacterIndexForPoint (CGPoint point, NSTextContainer container, ref nfloat fractionOfDistanceBetweenInsertionPoints);
#else
		nuint CharacterIndexForPoint (CGPoint point, NSTextContainer container, ref nfloat partialFraction);
#endif
#endif

#if XAMCORE_4_0 || !MONOMAC
		[Protected]
#endif
		[Export ("getLineFragmentInsertionPointsForCharacterAtIndex:alternatePositions:inDisplayOrder:positions:characterIndexes:")]
#if XAMCORE_4_0 || !MONOMAC
		nuint GetLineFragmentInsertionPoints (nuint characterIndex, bool alternatePositions, bool inDisplayOrder, IntPtr positions, IntPtr characterIndexes);
#else
		nuint GetLineFragmentInsertionPoints (nuint charIndex, bool aFlag, bool dFlag, IntPtr positions, IntPtr charIndexes);
#endif

		/* GetTemporaryAttributes (NSUInteger, nullable NSRangePointer) */

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Protected]
		[Export ("temporaryAttributesAtCharacterIndex:effectiveRange:")]
		NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, IntPtr effectiveCharacterRange);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Wrap ("GetTemporaryAttributes (characterIndex, IntPtr.Zero)")]
		NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Sealed]
		[Export ("temporaryAttributesAtCharacterIndex:effectiveRange:")]
		NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, out NSRange effectiveCharacterRange);

		[NoiOS, NoTV, NoWatch]
		[NoMacCatalyst]
		[Export ("setTemporaryAttributes:forCharacterRange:")]
		void SetTemporaryAttributes (NSDictionary attrs, NSRange charRange);

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("addTemporaryAttributes:forCharacterRange:")]
#if XAMCORE_4_0
		void AddTemporaryAttributes (NSDictionary<NSString, NSObject> attributes, NSRange characterRange);
#else
		void AddTemporaryAttributes (NSDictionary attrs, NSRange charRange);
#endif

		// This API can take an NSString or managed string, but some related API
		// takes a generic dictionary that can't use a managed string, so for symmetry
		// provide an NSString overload as well.
#if !XAMCORE_4_0
		[Sealed]
#endif
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("removeTemporaryAttribute:forCharacterRange:")]
		void RemoveTemporaryAttribute (NSString attributeName, NSRange characterRange);

#if XAMCORE_4_0
		[Sealed]
#endif
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("removeTemporaryAttribute:forCharacterRange:")]
#if XAMCORE_4_0
		void RemoveTemporaryAttribute (string attributeName, NSRange characterRange);
#else
		void RemoveTemporaryAttribute (string attrName, NSRange charRange);
#endif

		/* GetTemporaryAttribute (NSString, NSUInteger, nullable NSRangePointer) */
		[Protected]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("temporaryAttribute:atCharacterIndex:effectiveRange:")]
		NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, /* nullable NSRangePointer */ IntPtr effectiveRange);

		[Wrap ("GetTemporaryAttribute (attributeName, characterIndex, IntPtr.Zero)")]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex);

		[Sealed]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("temporaryAttribute:atCharacterIndex:effectiveRange:")]
		NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, /* nullable NSRangePointer */ out NSRange effectiveRange);

		/* GetTemporaryAttribute (NSString, NSUInteger, nullable NSRangePointer, NSRange) */

		[Protected]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("temporaryAttribute:atCharacterIndex:longestEffectiveRange:inRange:")]
		NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, /* nullable NSRangePointer */ IntPtr longestEffectiveRange, NSRange rangeLimit);

		[Wrap ("GetTemporaryAttribute (attributeName, characterIndex, IntPtr.Zero, rangeLimit)")]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, NSRange rangeLimit);

		[Sealed]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("temporaryAttribute:atCharacterIndex:longestEffectiveRange:inRange:")]
		NSObject GetTemporaryAttribute (NSString attributeName, nuint characterIndex, /* nullable NSRangePointer */ out NSRange longestEffectiveRange, NSRange rangeLimit);

		/* GetTemporaryAttributes (NSUInteger, nullable NSRangePointer, NSRange) */

		[Protected]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("temporaryAttributesAtCharacterIndex:longestEffectiveRange:inRange:")]
		NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, /* nullable NSRangePointer */ IntPtr longestEffectiveRange, NSRange rangeLimit);

		[Wrap ("GetTemporaryAttributes (characterIndex, IntPtr.Zero, rangeLimit)")]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, NSRange rangeLimit);

		[Sealed]
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("temporaryAttributesAtCharacterIndex:longestEffectiveRange:inRange:")]
		NSDictionary<NSString, NSObject> GetTemporaryAttributes (nuint characterIndex, /* nullable NSRangePointer */ out NSRange longestEffectiveRange, NSRange rangeLimit);

		// This method can take an NSString or managed string, but some related API
		// takes a generic dictionary that can't use a managed string, so for symmetry
		// provide an NSString overload as well.
#if !XAMCORE_4_0
		[Sealed]
#endif
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("addTemporaryAttribute:value:forCharacterRange:")]
		void AddTemporaryAttribute (NSString attributeName, NSObject value, NSRange characterRange);

#if XAMCORE_4_0
		[Sealed]
#endif
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("addTemporaryAttribute:value:forCharacterRange:")]
#if XAMCORE_4_0
		void AddTemporaryAttribute (string attributeName, NSObject value, NSRange characterRange);
#else
		void AddTemporaryAttribute (string attrName, NSObject value, NSRange charRange);
#endif

#if !XAMCORE_4_0
		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11)]
		[Export ("substituteFontForFont:")]
		NSFont SubstituteFontForFont (NSFont originalFont);
#endif

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("defaultLineHeightForFont:")]
#if XAMCORE_4_0
		nfloat GetDefaultLineHeight (NSFont font);
#else
		nfloat DefaultLineHeightForFont (NSFont theFont);
#endif

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("defaultBaselineOffsetForFont:")]
#if XAMCORE_4_0
		nfloat GetDefaultBaselineOffset (NSFont font);
#else
		nfloat DefaultBaselineOffsetForFont (NSFont theFont);
#endif

		[NullAllowed]
		[Export ("textStorage", ArgumentSemantic.Assign)]
		NSTextStorage TextStorage { get; set; }

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11)]
		[Export ("glyphGenerator", ArgumentSemantic.Retain)]
		NSGlyphGenerator GlyphGenerator { get; set; }

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("typesetter", ArgumentSemantic.Retain)]
		NSTypesetter Typesetter { get; set; }

		[Export ("delegate", ArgumentSemantic.Assign)][NullAllowed]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		INSLayoutManagerDelegate Delegate { get; set; }

		[NoiOS][NoTV]
		[Export ("backgroundLayoutEnabled")]
		bool BackgroundLayoutEnabled { get; set; }

		[NoiOS][NoTV]
		[Availability (Deprecated = Platform.Mac_10_11)]
		[Export ("usesScreenFonts")]
		bool UsesScreenFonts { get; set; }

		[Export ("showsInvisibleCharacters")]
		bool ShowsInvisibleCharacters { get; set; }

		[Export ("showsControlCharacters")]
		bool ShowsControlCharacters { get; set; }

		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Please use 'UsesDefaultHyphenation' or 'NSParagraphStyle.HyphenationFactor' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Please use 'UsesDefaultHyphenation' or 'NSParagraphStyle.HyphenationFactor' instead.")]
		[Deprecated (PlatformName.WatchOS, 6, 0, message: "Please use 'UsesDefaultHyphenation' or 'NSParagraphStyle.HyphenationFactor' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Please use 'UsesDefaultHyphenation' or 'NSParagraphStyle.HyphenationFactor' instead.")]
		[NoMacCatalyst]
		[Export ("hyphenationFactor")]
#if MONOMAC
		float HyphenationFactor { get; set; } /* This is defined as float in AppKit headers. */
#else
		nfloat HyphenationFactor { get; set; } /* This is defined as CGFloat in UIKit headers. */
#endif

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("defaultAttachmentScaling")]
		NSImageScaling DefaultAttachmentScaling { get; set; }

		[NoiOS][NoTV]
		[NoMacCatalyst]
		[Export ("typesetterBehavior")]
		NSTypesetterBehavior TypesetterBehavior { get; set; }

		[Export ("allowsNonContiguousLayout")]
		bool AllowsNonContiguousLayout { get; set; }

		[Export ("usesFontLeading")]
		bool UsesFontLeading { get; set; }

		[Export ("drawBackgroundForGlyphRange:atPoint:")]
#if XAMCORE_4_0
		void DrawBackground (NSRange glyphsToShow, CGPoint origin);
#else
		void DrawBackgroundForGlyphRange (NSRange glyphsToShow, CGPoint origin);
#endif

		[Export ("drawGlyphsForGlyphRange:atPoint:")]
#if XAMCORE_4_0 || !MONOMAC
		void DrawGlyphs (NSRange glyphsToShow, CGPoint origin);
#else
		void DrawGlyphsForGlyphRange (NSRange glyphsToShow, CGPoint origin);
#endif

		[Protected] // Class can be subclassed, and most methods can be overridden.
		[Mac (10,10)]
		[Export ("getGlyphsInRange:glyphs:properties:characterIndexes:bidiLevels:")]
		nuint GetGlyphs (NSRange glyphRange, IntPtr glyphBuffer, IntPtr properties, IntPtr characterIndexBuffer, IntPtr bidiLevelBuffer);

#if !XAMCORE_4_0 && !MONOMAC
		[Sealed]
#endif
		[Mac (10,10)]
		[Export ("propertyForGlyphAtIndex:")]
		NSGlyphProperty GetProperty (nuint glyphIndex);

#if !XAMCORE_4_0 && !MONOMAC
		[Obsolete ("Use 'GetProperty' instead.")]
		[Export ("propertyForGlyphAtIndex:")]
		NSGlyphProperty PropertyForGlyphAtIndex (nuint glyphIndex);
#endif

		[Mac (10,11)]
		[iOS (9,0)] // Show up in the iOS 7.0 headers, but they can't be found at runtime until iOS 9.
		[Export ("CGGlyphAtIndex:isValidIndex:")]
#if XAMCORE_4_0
		CGGlyph GetGlyph (nuint glyphIndex, out bool isValidIndex);
#elif MONOMAC
		CGGlyph GetCGGlyph (nuint glyphIndex, out bool isValidIndex);
#else
		CGGlyph GetGlyph (nuint glyphIndex, ref bool isValidIndex);
#endif

		[Mac (10,11)]
		[iOS (9,0)] // Show up in the iOS 7.0 headers, but they can't be found at runtime until iOS 9.
		[Export ("CGGlyphAtIndex:")]
#if XAMCORE_4_0
		CGGlyph GetGlyph (nuint glyphIndex);
#elif MONOMAC
		CGGlyph GetCGGlyph (nuint glyphIndex);
#else
		CGGlyph GetGlyph (nuint glyphIndex);
#endif

		[Mac (10,11)]
		[Export ("processEditingForTextStorage:edited:range:changeInLength:invalidatedRange:")]
#if XAMCORE_4_0
		void ProcessEditing (NSTextStorage textStorage, NSTextStorageEditActions editMask, NSRange newCharacterRange, /* NSInteger */ nint delta, NSRange invalidatedCharacterRange);
#else
		void ProcessEditing (NSTextStorage textStorage, NSTextStorageEditActions editMask, NSRange newCharRange, /* NSInteger */ nint delta, NSRange invalidatedCharRange);
#endif

		// This method can only be called from
		// NSLayoutManagerDelegate.ShouldGenerateGlyphs, and that method takes
		// the same IntPtr arguments as this one. This means that creating a
		// version of this method with nice(r) types (arrays instead of
		// IntPtr) is useless, since what the caller has is IntPtrs (from the
		// ShouldGenerateGlyphs parameters). We can revisit this if we ever
		// fix the generator to have support for C-style arrays.
		[Mac (10,11)]
		[Export ("setGlyphs:properties:characterIndexes:font:forGlyphRange:")]
#if XAMCORE_4_0
		void SetGlyphs (IntPtr glyphs, IntPtr properties, IntPtr characterIndexes, NSFont font, NSRange glyphRange);
#else
		void SetGlyphs (IntPtr glyphs, IntPtr props, IntPtr charIndexes, NSFont aFont, NSRange glyphRange);
#endif

#if !(XAMCORE_4_0 || MONOMAC)
		[Sealed]
#endif
		[Mac (10,11)]
		[Export ("truncatedGlyphRangeInLineFragmentForGlyphAtIndex:")]
		NSRange GetTruncatedGlyphRangeInLineFragment (nuint glyphIndex);

#if !(XAMCORE_4_0 || MONOMAC)
		[Obsolete ("Use 'GetTruncatedGlyphRangeInLineFragment' instead.")]
		[Mac (10,11)]
		[Export ("truncatedGlyphRangeInLineFragmentForGlyphAtIndex:")]
		NSRange TruncatedGlyphRangeInLineFragmentForGlyphAtIndex (nuint glyphIndex);
#endif

		[Mac (10,11)]
		[Export ("enumerateLineFragmentsForGlyphRange:usingBlock:")]
		void EnumerateLineFragments (NSRange glyphRange, NSTextLayoutEnumerateLineFragments callback);

		[Mac (10,11)]
		[Export ("enumerateEnclosingRectsForGlyphRange:withinSelectedGlyphRange:inTextContainer:usingBlock:")]
		void EnumerateEnclosingRects (NSRange glyphRange, NSRange selectedRange, NSTextContainer textContainer, NSTextLayoutEnumerateEnclosingRects callback);

		[Deprecated (PlatformName.MacOSX, 10, 15, message: "Use the overload that takes 'nint glyphCount' instead.")]
		[Deprecated (PlatformName.iOS, 13, 0, message: "Use the overload that takes 'nint glyphCount' instead.")]
		[Deprecated (PlatformName.WatchOS, 6, 0, message: "Use the overload that takes 'nint glyphCount' instead.")]
		[Deprecated (PlatformName.TvOS, 13, 0, message: "Use the overload that takes 'nint glyphCount' instead.")]
		[NoMacCatalyst]
		[Protected] // Can be overridden
		[Export ("showCGGlyphs:positions:count:font:matrix:attributes:inContext:")]
		void ShowGlyphs (IntPtr glyphs, IntPtr positions, nuint glyphCount, NSFont font, CGAffineTransform textMatrix, NSDictionary attributes, CGContext graphicsContext);

		[Watch (6,0), TV (13,0), Mac (10,15), iOS (13,0)]
		[Protected] // Can be overridden
		[Export ("showCGGlyphs:positions:count:font:textMatrix:attributes:inContext:")]
		void ShowGlyphs (IntPtr glyphs, IntPtr positions, nint glyphCount, NSFont font, CGAffineTransform textMatrix, NSDictionary attributes, CGContext graphicsContext);

		// Unfortunately we can't provide a nicer API for this, because it uses C-style arrays.
		// And providing a nicer overload when it's only purpose is to be overridden is useless.
		[Advice ("This method should never be called, only overridden.")] // According to Apple's documentation
		[Protected]
		[Export ("fillBackgroundRectArray:count:forCharacterRange:color:")]
		void FillBackground (IntPtr rectArray, nuint rectCount, NSRange characterRange, NSColor color);

 		[Export ("drawUnderlineForGlyphRange:underlineType:baselineOffset:lineFragmentRect:lineFragmentGlyphRange:containerOrigin:")]
 		void DrawUnderline (NSRange glyphRange, NSUnderlineStyle underlineVal, nfloat baselineOffset, CGRect lineRect, NSRange lineGlyphRange, CGPoint containerOrigin);

 		[Export ("underlineGlyphRange:underlineType:lineFragmentRect:lineFragmentGlyphRange:containerOrigin:")]
 		void Underline (NSRange glyphRange, NSUnderlineStyle underlineVal, CGRect lineRect, NSRange lineGlyphRange, CGPoint containerOrigin);

 		[Export ("drawStrikethroughForGlyphRange:strikethroughType:baselineOffset:lineFragmentRect:lineFragmentGlyphRange:containerOrigin:")]
 		void DrawStrikethrough (NSRange glyphRange, NSUnderlineStyle strikethroughVal, nfloat baselineOffset, CGRect lineRect, NSRange lineGlyphRange, CGPoint containerOrigin);

 		[Export ("strikethroughGlyphRange:strikethroughType:lineFragmentRect:lineFragmentGlyphRange:containerOrigin:")]
 		void Strikethrough (NSRange glyphRange, NSUnderlineStyle strikethroughVal, CGRect lineRect, NSRange lineGlyphRange, CGPoint containerOrigin);

		[NoiOS][NoTV]
		[NoMacCatalyst]
 		[Export ("showAttachmentCell:inRect:characterIndex:")]
		void ShowAttachmentCell (NSCell cell, CGRect rect, nuint characterIndex);

		[Mac (10, 14)]
		[TV (12, 0), iOS (12, 0)]
		[Export ("limitsLayoutForSuspiciousContents")]
		bool LimitsLayoutForSuspiciousContents { get; set; }

		[Mac (10,15)]
		[TV (13,0), iOS (13,0)]
		[Export ("usesDefaultHyphenation")]
		bool UsesDefaultHyphenation { get; set; }
	}

	[NoiOS][NoWatch][NoTV]
	[NoMacCatalyst]
	[Category]
	[BaseType (typeof (NSLayoutManager))]
	interface NSLayoutManager_NSTextViewSupport {
		[Export ("rulerMarkersForTextView:paragraphStyle:ruler:")]
		NSRulerMarker[] GetRulerMarkers (NSTextView textView, NSParagraphStyle paragraphStyle, NSRulerView ruler);

		[return: NullAllowed]
		[Export ("rulerAccessoryViewForTextView:paragraphStyle:ruler:enabled:")]
		NSView GetRulerAccessoryView (NSTextView textView, NSParagraphStyle paragraphStyle, NSRulerView ruler, bool enabled);

		[Export ("layoutManagerOwnsFirstResponderInWindow:")]
		bool LayoutManagerOwnsFirstResponder (NSWindow window);

		[return: NullAllowed]
		[Export ("firstTextView", ArgumentSemantic.Assign)]
		NSTextView GetFirstTextView ();

		[return: NullAllowed]
		[Export ("textViewForBeginningOfSelection")]
		NSTextView GetTextViewForBeginningOfSelection ();
	}

	interface INSLayoutManagerDelegate {}

	[NoWatch] // Header not present in watchOS SDK.
	[iOS (7,0)]
	[BaseType (typeof (NSObject))]
	[Model]
	[Protocol]
	interface NSLayoutManagerDelegate {
		[Export ("layoutManagerDidInvalidateLayout:")]
#if MONOMAC && !XAMCORE_4_0
		void LayoutInvalidated (NSLayoutManager sender);
#else
		void DidInvalidatedLayout (NSLayoutManager sender);
#endif

		[iOS (7,0)]
		[Export ("layoutManager:didCompleteLayoutForTextContainer:atEnd:")]
#if XAMCORE_4_0 || !MONOMAC
		void DidCompleteLayout (NSLayoutManager layoutManager, NSTextContainer textContainer, bool layoutFinishedFlag);
#else
		void LayoutCompleted (NSLayoutManager layoutManager, NSTextContainer textContainer, bool layoutFinishedFlag);
#endif

		[NoiOS][NoTV]
		[Export ("layoutManager:shouldUseTemporaryAttributes:forDrawingToScreen:atCharacterIndex:effectiveRange:")]
#if XAMCORE_4_0
		NSDictionary<NSString, NSObject> ShouldUseTemporaryAttributes (NSLayoutManager layoutManager, NSDictionary<NSString, NSObject> temporaryAttributes, bool drawingToScreen, nuint characterIndex, ref NSRange effectiveCharacterRange);
#else
		NSDictionary ShouldUseTemporaryAttributes (NSLayoutManager layoutManager, NSDictionary temporaryAttributes, bool drawingToScreen, nint charIndex, IntPtr effectiveCharRange);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:shouldGenerateGlyphs:properties:characterIndexes:font:forGlyphRange:")]
#if XAMCORE_4_0
		nuint ShouldGenerateGlyphs (NSLayoutManager layoutManager, IntPtr glyphBuffer, IntPtr properties, IntPtr characterIndexes, NSFont font, NSRange glyphRange);
#else
		nuint ShouldGenerateGlyphs (NSLayoutManager layoutManager, IntPtr glyphBuffer, IntPtr props, IntPtr charIndexes, NSFont aFont, NSRange glyphRange);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:lineSpacingAfterGlyphAtIndex:withProposedLineFragmentRect:")]
#if XAMCORE_4_0 || MONOMAC
		nfloat GetLineSpacingAfterGlyph (NSLayoutManager layoutManager, nuint glyphIndex, CGRect rect);
#else
		nfloat LineSpacingAfterGlyphAtIndex (NSLayoutManager layoutManager, nuint glyphIndex, CGRect rect);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:paragraphSpacingBeforeGlyphAtIndex:withProposedLineFragmentRect:")]
#if XAMCORE_4_0 || MONOMAC
		nfloat GetParagraphSpacingBeforeGlyph (NSLayoutManager layoutManager, nuint glyphIndex, CGRect rect);
#else
		nfloat ParagraphSpacingBeforeGlyphAtIndex (NSLayoutManager layoutManager, nuint glyphIndex, CGRect rect);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:paragraphSpacingAfterGlyphAtIndex:withProposedLineFragmentRect:")]
#if XAMCORE_4_0 || MONOMAC
		nfloat GetParagraphSpacingAfterGlyph (NSLayoutManager layoutManager, nuint glyphIndex, CGRect rect);
#else
		nfloat ParagraphSpacingAfterGlyphAtIndex (NSLayoutManager layoutManager, nuint glyphIndex, CGRect rect);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:shouldUseAction:forControlCharacterAtIndex:")]
#if XAMCORE_4_0
		NSControlCharacterAction ShouldUseAction (NSLayoutManager layoutManager, NSControlCharacterAction action, nuint characterIndex);
#else
		NSControlCharacterAction ShouldUseAction (NSLayoutManager layoutManager, NSControlCharacterAction action, nuint charIndex);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:shouldBreakLineByWordBeforeCharacterAtIndex:")]
#if XAMCORE_4_0
		bool ShouldBreakLineByWordBeforeCharacter (NSLayoutManager layoutManager, nuint characterIndex);
#else
		bool ShouldBreakLineByWordBeforeCharacter (NSLayoutManager layoutManager, nuint charIndex);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:shouldBreakLineByHyphenatingBeforeCharacterAtIndex:")]
#if XAMCORE_4_0
		bool ShouldBreakLineByHyphenatingBeforeCharacter (NSLayoutManager layoutManager, nuint characterIndex);
#else
		bool ShouldBreakLineByHyphenatingBeforeCharacter (NSLayoutManager layoutManager, nuint charIndex);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:boundingBoxForControlGlyphAtIndex:forTextContainer:proposedLineFragment:glyphPosition:characterIndex:")]
#if XAMCORE_4_0
		CGRect GetBoundingBox (NSLayoutManager layoutManager, nuint glyphIndex, NSTextContainer textContainer, CGRect proposedRect, CGPoint glyphPosition, nuint characterIndex);
#elif MONOMAC
		CGRect GetBoundingBox (NSLayoutManager layoutManager, nuint glyphIndex, NSTextContainer textContainer, CGRect proposedRect, CGPoint glyphPosition, nuint charIndex);
#else
		CGRect BoundingBoxForControlGlyph (NSLayoutManager layoutManager, nuint glyphIndex, NSTextContainer textContainer, CGRect proposedRect, CGPoint glyphPosition, nuint charIndex);
#endif

		[Mac (10,11)]
		[Export ("layoutManager:textContainer:didChangeGeometryFromSize:")]
		void DidChangeGeometry (NSLayoutManager layoutManager, NSTextContainer textContainer, CGSize oldSize);

		[iOS (9,0)]
		[Mac (10,11)]
		[Export ("layoutManager:shouldSetLineFragmentRect:lineFragmentUsedRect:baselineOffset:inTextContainer:forGlyphRange:")]
		bool ShouldSetLineFragmentRect (NSLayoutManager layoutManager, ref CGRect lineFragmentRect, ref CGRect lineFragmentUsedRect, ref nfloat baselineOffset, NSTextContainer textContainer, NSRange glyphRange);
	}

	[NoWatch, TV (13,0), Mac (10,15), iOS (13,0)]
	[BaseType (typeof (NSObject))]
	interface NSDiffableDataSourceSnapshot<SectionIdentifierType, ItemIdentifierType> : NSCopying
		where SectionIdentifierType : NSObject
		where ItemIdentifierType : NSObject {

		[Export ("numberOfItems")]
		nint NumberOfItems { get; }

		[Export ("numberOfSections")]
		nint NumberOfSections { get; }

		[Export ("sectionIdentifiers")]
		SectionIdentifierType [] SectionIdentifiers { get; }

		[Export ("itemIdentifiers")]
		ItemIdentifierType [] ItemIdentifiers { get; }

		[Export ("numberOfItemsInSection:")]
		nint GetNumberOfItems (SectionIdentifierType sectionIdentifier);

		[Export ("itemIdentifiersInSectionWithIdentifier:")]
		ItemIdentifierType [] GetItemIdentifiersInSection (SectionIdentifierType sectionIdentifier);

		[Export ("sectionIdentifierForSectionContainingItemIdentifier:")]
		[return: NullAllowed]
		SectionIdentifierType GetSectionIdentifierForSection (ItemIdentifierType itemIdentifier);

		[Export ("indexOfItemIdentifier:")]
		nint GetIndex (ItemIdentifierType itemIdentifier);

		[Export ("indexOfSectionIdentifier:")]
		nint GetIndex (SectionIdentifierType sectionIdentifier);

		[Export ("appendItemsWithIdentifiers:")]
		void AppendItems (ItemIdentifierType [] identifiers);

		[Export ("appendItemsWithIdentifiers:intoSectionWithIdentifier:")]
		void AppendItems (ItemIdentifierType [] identifiers, SectionIdentifierType sectionIdentifier);

		[Export ("insertItemsWithIdentifiers:beforeItemWithIdentifier:")]
		void InsertItemsBefore (ItemIdentifierType [] identifiers, ItemIdentifierType itemIdentifier);

		[Export ("insertItemsWithIdentifiers:afterItemWithIdentifier:")]
		void InsertItemsAfter (ItemIdentifierType [] identifiers, ItemIdentifierType itemIdentifier);

		[Export ("deleteItemsWithIdentifiers:")]
		void DeleteItems (ItemIdentifierType [] identifiers);

		[Export ("deleteAllItems")]
		void DeleteAllItems ();

		[Export ("moveItemWithIdentifier:beforeItemWithIdentifier:")]
		void MoveItemBefore (ItemIdentifierType fromIdentifier, ItemIdentifierType toIdentifier);

		[Export ("moveItemWithIdentifier:afterItemWithIdentifier:")]
		void MoveItemAfter (ItemIdentifierType fromIdentifier, ItemIdentifierType toIdentifier);

		[Export ("reloadItemsWithIdentifiers:")]
		void ReloadItems (ItemIdentifierType [] identifiers);

		[Export ("appendSectionsWithIdentifiers:")]
		void AppendSections (SectionIdentifierType [] sectionIdentifiers);

		[Export ("insertSectionsWithIdentifiers:beforeSectionWithIdentifier:")]
		void InsertSectionsBefore (SectionIdentifierType [] sectionIdentifiers, SectionIdentifierType toSectionIdentifier);

		[Export ("insertSectionsWithIdentifiers:afterSectionWithIdentifier:")]
		void InsertSectionsAfter (SectionIdentifierType [] sectionIdentifiers, SectionIdentifierType toSectionIdentifier);

		[Export ("deleteSectionsWithIdentifiers:")]
		void DeleteSections (SectionIdentifierType [] sectionIdentifiers);

		[Export ("moveSectionWithIdentifier:beforeSectionWithIdentifier:")]
		void MoveSectionBefore (SectionIdentifierType fromSectionIdentifier, SectionIdentifierType toSectionIdentifier);

		[Export ("moveSectionWithIdentifier:afterSectionWithIdentifier:")]
		void MoveSectionAfter (SectionIdentifierType fromSectionIdentifier, SectionIdentifierType toSectionIdentifier);

		[Export ("reloadSectionsWithIdentifiers:")]
		void ReloadSections (SectionIdentifierType [] sectionIdentifiers);
	}

	[ThreadSafe]
	[BaseType (typeof (NSObject))]
	interface NSParagraphStyle : NSSecureCoding, NSMutableCopying {
		[Export ("lineSpacing")]
		nfloat LineSpacing { get; [NotImplemented] set; }

		[Export ("paragraphSpacing")]
		nfloat ParagraphSpacing { get; [NotImplemented] set; }

		[Export ("alignment")]
		TextAlignment Alignment { get; [NotImplemented] set; }

		[Export ("headIndent")]
		nfloat HeadIndent { get; [NotImplemented] set; }

		[Export ("tailIndent")]
		nfloat TailIndent { get; [NotImplemented] set; }

		[Export ("firstLineHeadIndent")]
		nfloat FirstLineHeadIndent { get; [NotImplemented] set; }

		[Export ("minimumLineHeight")]
		nfloat MinimumLineHeight { get; [NotImplemented] set; }

		[Export ("maximumLineHeight")]
		nfloat MaximumLineHeight { get; [NotImplemented] set; }

		[Export ("lineBreakMode")]
		LineBreakMode LineBreakMode { get; [NotImplemented] set; }

		[Export ("baseWritingDirection")]
		NSWritingDirection BaseWritingDirection { get; [NotImplemented] set; }

		[Export ("lineHeightMultiple")]
		nfloat LineHeightMultiple { get; [NotImplemented] set; }

		[Export ("paragraphSpacingBefore")]
		nfloat ParagraphSpacingBefore { get; [NotImplemented] set; }

		[Export ("hyphenationFactor")]
		float HyphenationFactor { get; [NotImplemented] set; } // Returns a float, not nfloat.

		[Static]
		[Export ("defaultWritingDirectionForLanguage:")]
		NSWritingDirection GetDefaultWritingDirection ([NullAllowed] string languageName);

#if MONOMAC && !XAMCORE_4_0
		[Obsolete ("Use the 'GetDefaultWritingDirection' method instead.")]
		[Static]
		[Export ("defaultWritingDirectionForLanguage:")]
		NSWritingDirection DefaultWritingDirection ([NullAllowed] string languageName);
#endif

		[Static]
		[Export ("defaultParagraphStyle", ArgumentSemantic.Copy)]
		NSParagraphStyle Default { get; }

#if MONOMAC && !XAMCORE_4_0
		[Obsolete ("Use the 'Default' property instead.")]
		[Static]
		[Export ("defaultParagraphStyle", ArgumentSemantic.Copy)]
		NSParagraphStyle DefaultParagraphStyle { get; [NotImplemented] set; }
#endif

		[iOS (7,0)]
		[Export ("defaultTabInterval")]
		nfloat DefaultTabInterval { get; [NotImplemented] set; }

		[iOS (7,0)]
		[Export ("tabStops", ArgumentSemantic.Copy)]
		[NullAllowed]
		NSTextTab[] TabStops { get; [NotImplemented] set; }

		[iOS (9,0)]
		[Mac (10,11)]
		[Export ("allowsDefaultTighteningForTruncation")]
		bool AllowsDefaultTighteningForTruncation { get; [NotImplemented] set; }

		[NoiOS, NoTV, NoWatch]
		[Export ("textBlocks")]
#if XAMCORE_4_0
		NSTextBlock [] TextBlocks { get; [NotImplemented] set; }
#else
		NSTextTableBlock [] TextBlocks { get; [NotImplemented] set; }
#endif

		[NoiOS, NoTV, NoWatch]
		[Export ("textLists")]
		NSTextList[] TextLists { get; [NotImplemented] set; }

		[NoiOS, NoTV, NoWatch]
		[Export ("tighteningFactorForTruncation")]
		float TighteningFactorForTruncation { get; [NotImplemented] set; } /* float, not CGFloat */

		[NoiOS, NoTV, NoWatch]
		[Export ("headerLevel")]
		nint HeaderLevel { get; [NotImplemented] set; }

		[Mac (11,0), Watch (7,0), TV (14,0), iOS (14,0)]
		[Export ("lineBreakStrategy")]
		NSLineBreakStrategy LineBreakStrategy { get; [NotImplemented] set; }
	}

	[ThreadSafe]
	[BaseType (typeof (NSParagraphStyle))]
	interface NSMutableParagraphStyle {
		[Export ("lineSpacing")]
		[Override]
		nfloat LineSpacing { get; set; }

		[Export ("alignment")]
		[Override]
		TextAlignment Alignment { get; set; }

		[Export ("headIndent")]
		[Override]
		nfloat HeadIndent { get; set; }

		[Export ("tailIndent")]
		[Override]
		nfloat TailIndent { get; set; }

		[Export ("firstLineHeadIndent")]
		[Override]
		nfloat FirstLineHeadIndent { get; set; }

		[Export ("minimumLineHeight")]
		[Override]
		nfloat MinimumLineHeight { get; set; }

		[Export ("maximumLineHeight")]
		[Override]
		nfloat MaximumLineHeight { get; set; }

		[Export ("lineBreakMode")]
		[Override]
		LineBreakMode LineBreakMode { get; set; }

		[Export ("baseWritingDirection")]
		[Override]
		NSWritingDirection BaseWritingDirection { get; set; }

		[Export ("lineHeightMultiple")]
		[Override]
		nfloat LineHeightMultiple { get; set; }

		[Export ("paragraphSpacing")]
		[Override]
		nfloat ParagraphSpacing { get; set; }

		[Export ("paragraphSpacingBefore")]
		[Override]
		nfloat ParagraphSpacingBefore { get; set; }

		[Export ("hyphenationFactor")]
		[Override]
		float HyphenationFactor { get; set; } // Returns a float, not nfloat.

		[iOS (7,0)]
		[Export ("defaultTabInterval")]
		[Override]
		nfloat DefaultTabInterval { get; set; }

		[iOS (7,0)]
		[Export ("tabStops", ArgumentSemantic.Copy)]
		[Override]
		[NullAllowed]
		NSTextTab[] TabStops { get; set; }

		[iOS (9,0)]
		[Mac (10,11)]
		[Override]
		[Export ("allowsDefaultTighteningForTruncation")]
		bool AllowsDefaultTighteningForTruncation { get; set; }

		[iOS (9,0)]
		[Export ("addTabStop:")]
		void AddTabStop (NSTextTab textTab);

		[iOS (9,0)]
		[Export ("removeTabStop:")]
		void RemoveTabStop (NSTextTab textTab);

		[iOS (9,0)]
		[Export ("setParagraphStyle:")]
		void SetParagraphStyle (NSParagraphStyle paragraphStyle);

		[NoiOS, NoTV, NoWatch]
		[NoMacCatalyst]
		[Override]
		[Export ("textBlocks")]
#if XAMCORE_4_0
		NSTextBlock [] TextBlocks { get; set; }
#else
		NSTextTableBlock [] TextBlocks { get; set; }
#endif

		[NoiOS, NoTV, NoWatch]
		[NoMacCatalyst]
		[Override]
		[Export ("textLists")]
		NSTextList [] TextLists { get; set; }

		[NoiOS, NoTV, NoWatch]
		[Export ("tighteningFactorForTruncation")]
		[Override]
		float TighteningFactorForTruncation { get; set; } /* float, not CGFloat */

		[NoiOS, NoTV, NoWatch]
		[NoMacCatalyst]
		[Export ("headerLevel")]
		[Override]
		nint HeaderLevel { get; set; }

		[Mac (11,0), Watch (7,0), TV (14,0), iOS (14,0)]
		[Override]
		[Export ("lineBreakStrategy", ArgumentSemantic.Assign)]
		NSLineBreakStrategy LineBreakStrategy { get; set; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	delegate NSCollectionLayoutGroupCustomItem [] NSCollectionLayoutGroupCustomItemProvider (INSCollectionLayoutEnvironment layoutEnvironment);

	[NoWatch, TV (13,0), iOS (13,0)]
	[BaseType (typeof (NSCollectionLayoutItem))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutGroup : NSCopying {

		[Static]
		[Export ("horizontalGroupWithLayoutSize:subitem:count:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutGroup CreateHorizontalGroup (NSCollectionLayoutSize layoutSize, NSCollectionLayoutItem subitem, nint count);
#else
		NSCollectionLayoutGroup CreateHorizontal (NSCollectionLayoutSize layoutSize, NSCollectionLayoutItem subitem, nint count);
#endif

		[Static]
		[Export ("horizontalGroupWithLayoutSize:subitems:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutGroup CreateHorizontalGroup (NSCollectionLayoutSize layoutSize, NSCollectionLayoutItem [] subitems);
#else
		NSCollectionLayoutGroup CreateHorizontal (NSCollectionLayoutSize layoutSize, params NSCollectionLayoutItem [] subitems);
#endif

		[Static]
		[Export ("verticalGroupWithLayoutSize:subitem:count:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutGroup CreateVerticalGroup (NSCollectionLayoutSize layoutSize, NSCollectionLayoutItem subitem, nint count);
#else
		NSCollectionLayoutGroup CreateVertical (NSCollectionLayoutSize layoutSize, NSCollectionLayoutItem subitem, nint count);
#endif

		[Static]
		[Export ("verticalGroupWithLayoutSize:subitems:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutGroup CreateVerticalGroup (NSCollectionLayoutSize layoutSize, NSCollectionLayoutItem [] subitems);
#else
		NSCollectionLayoutGroup CreateVertical (NSCollectionLayoutSize layoutSize, params NSCollectionLayoutItem [] subitems);
#endif

		[Static]
		[Export ("customGroupWithLayoutSize:itemProvider:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutGroup CreateCustomGroup (NSCollectionLayoutSize layoutSize, NSCollectionLayoutGroupCustomItemProvider itemProvider);
#else
		NSCollectionLayoutGroup CreateCustom (NSCollectionLayoutSize layoutSize, NSCollectionLayoutGroupCustomItemProvider itemProvider);
#endif

		[Export ("supplementaryItems", ArgumentSemantic.Copy)]
		NSCollectionLayoutSupplementaryItem [] SupplementaryItems { get; set; }

		[NullAllowed, Export ("interItemSpacing", ArgumentSemantic.Copy)]
		NSCollectionLayoutSpacing InterItemSpacing { get; set; }

		[Export ("subitems")]
		NSCollectionLayoutItem [] Subitems { get; }

		[Export ("visualDescription")]
		string VisualDescription { get; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	delegate void NSCollectionLayoutSectionVisibleItemsInvalidationHandler (INSCollectionLayoutVisibleItem [] visibleItems, CGPoint contentOffset, INSCollectionLayoutEnvironment layoutEnvironment);

	[Mac (10,15)]
	[NoWatch, TV (13,0), iOS (13,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutSection : NSCopying {

		[Static]
		[Export ("sectionWithGroup:")]
		NSCollectionLayoutSection Create (NSCollectionLayoutGroup group);

		[Export ("contentInsets", ArgumentSemantic.Assign)]
		NSDirectionalEdgeInsets ContentInsets { get; set; }

		[Export ("interGroupSpacing")]
		nfloat InterGroupSpacing { get; set; }

		[NoMac]
		[MacCatalyst (14,0)]
		[TV (14,0), iOS (14,0)]
		[Export ("contentInsetsReference", ArgumentSemantic.Assign)]
		UIContentInsetsReference ContentInsetsReference { get; set; }

		[Export ("orthogonalScrollingBehavior", ArgumentSemantic.Assign)]
		CollectionLayoutSectionOrthogonalScrollingBehavior OrthogonalScrollingBehavior { get; set; }

		[Export ("boundarySupplementaryItems", ArgumentSemantic.Copy)]
		NSCollectionLayoutBoundarySupplementaryItem [] BoundarySupplementaryItems { get; set; }

		[Export ("supplementariesFollowContentInsets")]
		bool SupplementariesFollowContentInsets { get; set; }

		[NullAllowed, Export ("visibleItemsInvalidationHandler", ArgumentSemantic.Copy)]
		NSCollectionLayoutSectionVisibleItemsInvalidationHandler VisibleItemsInvalidationHandler { get; set; }

		[Export ("decorationItems", ArgumentSemantic.Copy)]
		NSCollectionLayoutDecorationItem [] DecorationItems { get; set; }

		// NSCollectionLayoutSection (UICollectionLayoutListSection) category
		[NoMac]
		[MacCatalyst (14,0)]
		[TV (14,0), iOS (14,0)]
		[Static]
		[Export ("sectionWithListConfiguration:layoutEnvironment:")]
		NSCollectionLayoutSection GetSection (UICollectionLayoutListConfiguration listConfiguration, INSCollectionLayoutEnvironment layoutEnvironment);
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[MacCatalyst (13,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutGroupCustomItem : NSCopying
	{
		[Static]
		[Export ("customItemWithFrame:")]
		NSCollectionLayoutGroupCustomItem Create (CGRect frame);

		[Static]
		[Export ("customItemWithFrame:zIndex:")]
		NSCollectionLayoutGroupCustomItem Create (CGRect frame, nint zIndex);

		[Export ("frame")]
		CGRect Frame { get; }

		[Export ("zIndex")]
		nint ZIndex { get; }
	}

	interface INSCollectionLayoutContainer { }

	[NoWatch, TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[MacCatalyst (13,0)]
	[Protocol]
	interface NSCollectionLayoutContainer
	{
		[Abstract]
		[Export ("contentSize")]
		CGSize ContentSize { get; }

		[Abstract]
		[Export ("effectiveContentSize")]
		CGSize EffectiveContentSize { get; }

		[Abstract]
		[Export ("contentInsets")]
		NSDirectionalEdgeInsets ContentInsets { get; }

		[Abstract]
		[Export ("effectiveContentInsets")]
		NSDirectionalEdgeInsets EffectiveContentInsets { get; }
	}

	interface INSCollectionLayoutEnvironment { }

	[NoWatch, TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[MacCatalyst (13,0)]
	[Protocol]
	interface NSCollectionLayoutEnvironment {

		[Abstract]
		[Export ("container")]
		INSCollectionLayoutContainer Container { get; }

		[NoMac]
		[Abstract]
		[Export ("traitCollection")]
		UITraitCollection TraitCollection { get; }
	}

	interface INSCollectionLayoutVisibleItem { }

	[NoWatch, TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[MacCatalyst (13,0)]
	[Protocol]
	interface NSCollectionLayoutVisibleItem
#if !MONOMAC && !WATCH
	: UIDynamicItem
#endif
	{

		[Abstract]
		[Export ("alpha")]
		nfloat Alpha { get; set; }

		[Abstract]
		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Abstract]
		[Export ("hidden")]
		bool Hidden { [Bind ("isHidden")] get; set; }

#if MONOMAC
		// Inherited from UIDynamicItem for !MONOMAC
		[Abstract]
		[Export ("center", ArgumentSemantic.Assign)]
		CGPoint Center { get; set; }


		[Abstract]
		[Export ("bounds")]
		CGRect Bounds { get; }
#endif

		[NoMac]
		[Abstract]
		[Export ("transform3D", ArgumentSemantic.Assign)]
		CATransform3D Transform3D { get; set; }

		[Abstract]
		[Export ("name")]
		string Name { get; }

		[Abstract]
		[Export ("indexPath")]
		NSIndexPath IndexPath { get; }

		[Abstract]
		[Export ("frame")]
		CGRect Frame { get; }

		[Abstract]
		[Export ("representedElementCategory")]
		CollectionElementCategory RepresentedElementCategory {
			get;
		}

		[Abstract]
		[NullAllowed, Export ("representedElementKind")]
		string RepresentedElementKind { get; }
	}

	[NoWatch]
	[iOS (9,0)]
	[Mac (10,11)]
	[MacCatalyst (13,0)]
	[BaseType (typeof(NSObject))]
	[DisableDefaultCtor] // Handle is nil
	interface NSLayoutAnchor<AnchorType> : NSCopying, NSCoding
	{
		[Export ("constraintEqualToAnchor:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintEqualToAnchor (NSLayoutAnchor<AnchorType> anchor);
#else
		NSLayoutConstraint ConstraintEqualTo (NSLayoutAnchor<AnchorType> anchor);
#endif

		[Export ("constraintGreaterThanOrEqualToAnchor:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintGreaterThanOrEqualToAnchor (NSLayoutAnchor<AnchorType> anchor);
#else
		NSLayoutConstraint ConstraintGreaterThanOrEqualTo (NSLayoutAnchor<AnchorType> anchor);
#endif

		[Export ("constraintLessThanOrEqualToAnchor:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintLessThanOrEqualToAnchor (NSLayoutAnchor<AnchorType> anchor);
#else
		NSLayoutConstraint ConstraintLessThanOrEqualTo (NSLayoutAnchor<AnchorType> anchor);
#endif

		[Export ("constraintEqualToAnchor:constant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintEqualToAnchor (NSLayoutAnchor<AnchorType> anchor, nfloat constant);
#else
		NSLayoutConstraint ConstraintEqualTo (NSLayoutAnchor<AnchorType> anchor, nfloat constant);
#endif

		[Export ("constraintGreaterThanOrEqualToAnchor:constant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintGreaterThanOrEqualToAnchor (NSLayoutAnchor<AnchorType> anchor, nfloat constant);
#else
		NSLayoutConstraint ConstraintGreaterThanOrEqualTo (NSLayoutAnchor<AnchorType> anchor, nfloat constant);
#endif

		[Export ("constraintLessThanOrEqualToAnchor:constant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintLessThanOrEqualToAnchor (NSLayoutAnchor<AnchorType> anchor, nfloat constant);
#else
		NSLayoutConstraint ConstraintLessThanOrEqualTo (NSLayoutAnchor<AnchorType> anchor, nfloat constant);
#endif

		[NoiOS][NoMacCatalyst][NoTV][NoWatch]
		[Mac (10, 12)]
		[Export ("name")]
		string Name { get; }

		[NoiOS][NoMacCatalyst][NoTV][NoWatch]
		[Mac (10, 12)]
		[NullAllowed, Export ("item", ArgumentSemantic.Weak)]
		NSObject Item { get; }

		[NoiOS][NoMacCatalyst][NoTV][NoWatch]
		[Mac (10, 12)]
		[Export ("hasAmbiguousLayout")]
		bool HasAmbiguousLayout { get; }

		[NoiOS][NoMacCatalyst][NoTV][NoWatch]
		[Mac (10, 12)]
		[Export ("constraintsAffectingLayout")]
		NSLayoutConstraint[] ConstraintsAffectingLayout { get; }
	}

	[NoWatch]
	[iOS (9,0)]
	[TV (10,0)]
	[Mac (10,11)]
	[MacCatalyst (13,0)]
	[BaseType (typeof(NSLayoutAnchor<NSLayoutXAxisAnchor>))]
	[DisableDefaultCtor] // Handle is nil
	interface NSLayoutXAxisAnchor
	{
		[iOS (10,0)]
		[Mac (10,12)]
		[Export ("anchorWithOffsetToAnchor:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutDimension GetAnchorWithOffset (NSLayoutXAxisAnchor otherAnchor);
#else
		NSLayoutDimension CreateAnchorWithOffset (NSLayoutXAxisAnchor otherAnchor);
#endif

		[TV (11,0), iOS (11,0)]
		[Mac (11,0)]
		[Export ("constraintEqualToSystemSpacingAfterAnchor:multiplier:")]
		NSLayoutConstraint ConstraintEqualToSystemSpacingAfterAnchor (NSLayoutXAxisAnchor anchor, nfloat multiplier);

		[TV (11,0), iOS (11,0)]
		[Mac (11,0)]
		[Export ("constraintGreaterThanOrEqualToSystemSpacingAfterAnchor:multiplier:")]
		NSLayoutConstraint ConstraintGreaterThanOrEqualToSystemSpacingAfterAnchor (NSLayoutXAxisAnchor anchor, nfloat multiplier);

		[TV (11,0), iOS (11,0)]
		[Mac (11,0)]
		[Export ("constraintLessThanOrEqualToSystemSpacingAfterAnchor:multiplier:")]
		NSLayoutConstraint ConstraintLessThanOrEqualToSystemSpacingAfterAnchor (NSLayoutXAxisAnchor anchor, nfloat multiplier);
	}

	[NoWatch]
	[iOS (9,0)]
	[TV (10,0)]
	[Mac (10,11)]
	[MacCatalyst (13,0)]
	[BaseType (typeof(NSLayoutAnchor<NSLayoutYAxisAnchor>))]
	[DisableDefaultCtor] // Handle is nil
	interface NSLayoutYAxisAnchor
	{
		[iOS (10,0)]
		[Mac (10,12)]
		[Export ("anchorWithOffsetToAnchor:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutDimension GetAnchorWithOffset (NSLayoutYAxisAnchor otherAnchor);
#else
		NSLayoutDimension CreateAnchorWithOffset (NSLayoutYAxisAnchor otherAnchor);
#endif

		[TV (11,0), iOS (11,0)]
		[Mac (11,0)]
		[Export ("constraintEqualToSystemSpacingBelowAnchor:multiplier:")]
		NSLayoutConstraint ConstraintEqualToSystemSpacingBelowAnchor (NSLayoutYAxisAnchor anchor, nfloat multiplier);

		[TV (11,0), iOS (11,0)]
		[Mac (11,0)]
		[Export ("constraintGreaterThanOrEqualToSystemSpacingBelowAnchor:multiplier:")]
		NSLayoutConstraint ConstraintGreaterThanOrEqualToSystemSpacingBelowAnchor (NSLayoutYAxisAnchor anchor, nfloat multiplier);

		[TV (11,0), iOS (11,0)]
		[Mac (11,0)]
		[Export ("constraintLessThanOrEqualToSystemSpacingBelowAnchor:multiplier:")]
		NSLayoutConstraint ConstraintLessThanOrEqualToSystemSpacingBelowAnchor (NSLayoutYAxisAnchor anchor, nfloat multiplier);
	}

	[NoWatch]
	[iOS (9,0)]
	[Mac (10,11)]
	[BaseType (typeof(NSLayoutAnchor<NSLayoutDimension>))]
	[DisableDefaultCtor] // Handle is nil
	interface NSLayoutDimension
	{
		[Export ("constraintEqualToConstant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintEqualToConstant (nfloat constant);
#else
		NSLayoutConstraint ConstraintEqualTo (nfloat constant);
#endif

		[Export ("constraintGreaterThanOrEqualToConstant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintGreaterThanOrEqualToConstant (nfloat constant);
#else
		NSLayoutConstraint ConstraintGreaterThanOrEqualTo (nfloat constant);
#endif

		[Export ("constraintLessThanOrEqualToConstant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintLessThanOrEqualToConstant (nfloat constant);
#else
		NSLayoutConstraint ConstraintLessThanOrEqualTo (nfloat constant);
#endif

		[Export ("constraintEqualToAnchor:multiplier:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintEqualToAnchor (NSLayoutDimension anchor, nfloat multiplier);
#else
		NSLayoutConstraint ConstraintEqualTo (NSLayoutDimension anchor, nfloat multiplier);
#endif

		[Export ("constraintGreaterThanOrEqualToAnchor:multiplier:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintGreaterThanOrEqualToAnchor (NSLayoutDimension anchor, nfloat multiplier);
#else
		NSLayoutConstraint ConstraintGreaterThanOrEqualTo (NSLayoutDimension anchor, nfloat multiplier);
#endif

		[Export ("constraintLessThanOrEqualToAnchor:multiplier:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintLessThanOrEqualToAnchor (NSLayoutDimension anchor, nfloat multiplier);
#else
		NSLayoutConstraint ConstraintLessThanOrEqualTo (NSLayoutDimension anchor, nfloat multiplier);
#endif

		[Export ("constraintEqualToAnchor:multiplier:constant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintEqualToAnchor (NSLayoutDimension anchor, nfloat multiplier, nfloat constant);
#else
		NSLayoutConstraint ConstraintEqualTo (NSLayoutDimension anchor, nfloat multiplier, nfloat constant);
#endif

		[Export ("constraintGreaterThanOrEqualToAnchor:multiplier:constant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintGreaterThanOrEqualToAnchor (NSLayoutDimension anchor, nfloat multiplier, nfloat constant);
#else
		NSLayoutConstraint ConstraintGreaterThanOrEqualTo (NSLayoutDimension anchor, nfloat multiplier, nfloat constant);
#endif

		[Export ("constraintLessThanOrEqualToAnchor:multiplier:constant:")]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutConstraint ConstraintLessThanOrEqualToAnchor (NSLayoutDimension anchor, nfloat multiplier, nfloat constant);
#else
		NSLayoutConstraint ConstraintLessThanOrEqualTo (NSLayoutDimension anchor, nfloat multiplier, nfloat constant);
#endif
	}

	[NoWatch]
	[MacCatalyst (13,0)]
	[BaseType (typeof (NSObject))]
	interface NSLayoutConstraint
#if MONOMAC
		: NSAnimatablePropertyContainer
#endif
{
		[Static]
		[Export ("constraintsWithVisualFormat:options:metrics:views:")]
		NSLayoutConstraint [] FromVisualFormat (string format, NSLayoutFormatOptions formatOptions, [NullAllowed] NSDictionary metrics, NSDictionary views);

		[Static]
		[Export ("constraintWithItem:attribute:relatedBy:toItem:attribute:multiplier:constant:")]
		NSLayoutConstraint Create (INativeObject view1, NSLayoutAttribute attribute1, NSLayoutRelation relation, [NullAllowed] INativeObject view2, NSLayoutAttribute attribute2, nfloat multiplier, nfloat constant);

		[Export ("priority")]
		float Priority { get; set;  } // Returns a float, not nfloat.

		[Export ("shouldBeArchived")]
		bool ShouldBeArchived { get; set;  }

		[NullAllowed, Export ("firstItem", ArgumentSemantic.Assign)]
		NSObject FirstItem { get;  }

		[Export ("firstAttribute")]
		NSLayoutAttribute FirstAttribute { get;  }

		[Export ("relation")]
		NSLayoutRelation Relation { get;  }

		[Export ("secondItem", ArgumentSemantic.Assign)]
		[NullAllowed]
		NSObject SecondItem { get;  }

		[Export ("secondAttribute")]
		NSLayoutAttribute SecondAttribute { get;  }

		[Export ("multiplier")]
		nfloat Multiplier { get;  }

		[Export ("constant")]
		nfloat Constant { get; set;  }

		[iOS (8,0)]
		[Mac (10,10)]
		[Export ("active")]
		bool Active { [Bind ("isActive")] get; set; }

		[iOS (8,0)]
		[Mac (10,10)]
		[Static, Export ("activateConstraints:")]
		void ActivateConstraints (NSLayoutConstraint [] constraints);

		[iOS (8,0)]
		[Mac (10,10)]
		[Static, Export ("deactivateConstraints:")]
		void DeactivateConstraints (NSLayoutConstraint [] constraints);

		[Mac (10, 12)]
		[iOS (10,0), TV (10,0)]
		[Export ("firstAnchor", ArgumentSemantic.Copy)]
#if MONOMAC && !XAMCORE_4_0
		NSLayoutAnchor<NSObject> FirstAnchor { get; }
#else
		[Internal]
		IntPtr _FirstAnchor<AnchorType> ();
#endif

		[Mac (10, 12)]
		[iOS (10,0), TV (10,0)]
		[Export ("secondAnchor", ArgumentSemantic.Copy)]
#if MONOMAC && !XAMCORE_4_0
		[NullAllowed]
		NSLayoutAnchor<NSObject> SecondAnchor { get; }
#else
		[Internal]
		IntPtr _SecondAnchor<AnchorType> ();
#endif

		[NullAllowed, Export ("identifier")]
		string Identifier { get; set; }
	}

	[NoWatch]
	[Mac (10,11)]
	[MacCatalyst (13,0)]
	[Model]
	[Protocol]
	[BaseType (typeof (NSObject))]
	partial interface NSTextAttachmentContainer {
		[Abstract]
		[Export ("imageForBounds:textContainer:characterIndex:")]
		[return: NullAllowed]
#if MONOMAC && !XAMCORE_4_0
		Image GetImage (CGRect imageBounds, [NullAllowed] NSTextContainer textContainer, nuint charIndex);
#else
		Image GetImageForBounds (CGRect bounds, [NullAllowed] NSTextContainer textContainer, nuint characterIndex);
#endif

		[Abstract]
		[Export ("attachmentBoundsForTextContainer:proposedLineFragment:glyphPosition:characterIndex:")]
		CGRect GetAttachmentBounds ([NullAllowed] NSTextContainer textContainer, CGRect proposedLineFragment, CGPoint glyphPosition, nuint characterIndex);
	}

	[iOS (7,0)]
	[NoWatch]
	[MacCatalyst (13,0)]
	[BaseType (typeof (NSObject))]
	partial interface NSTextAttachment : NSTextAttachmentContainer, NSSecureCoding
#if !WATCH && !MONOMAC
	, UIAccessibilityContentSizeCategoryImageAdjusting
#endif // !WATCH
	{
		[NoiOS][NoTV][NoMacCatalyst]
		[Export ("initWithFileWrapper:")]
		IntPtr Constructor (NSFileWrapper fileWrapper);

		[Mac (10,11)]
		[DesignatedInitializer]
		[Export ("initWithData:ofType:")]
		[PostGet ("Contents")]
		IntPtr Constructor ([NullAllowed] NSData contentData, [NullAllowed] string uti);

		[Mac (10,11)]
		[NullAllowed]
		[Export ("contents", ArgumentSemantic.Retain)]
		NSData Contents { get; set; }

		[Mac (10,11)]
		[NullAllowed]
		[Export ("fileType", ArgumentSemantic.Retain)]
		string FileType { get; set; }

		[Mac (10,11)]
		[NullAllowed]
		[Export ("image", ArgumentSemantic.Retain)]
		Image Image { get; set; }

		[Mac (10,11)]
		[Export ("bounds")]
		CGRect Bounds { get; set; }

		[NullAllowed]
		[Export ("fileWrapper", ArgumentSemantic.Retain)]
		NSFileWrapper FileWrapper { get; set; }

		[NoiOS][NoTV][NoMacCatalyst]
		[Export ("attachmentCell", ArgumentSemantic.Retain)]
		NSTextAttachmentCell AttachmentCell { get; set; }

		[NoMac]
		[Watch (6,0), TV (13,0), iOS (13,0)]
		[Static]
		[Export ("textAttachmentWithImage:")]
		NSTextAttachment Create (Image image);
	}

	[NoWatch]
	[MacCatalyst (13,0)]
	[iOS (7,0)]
	[BaseType (typeof (NSMutableAttributedString), Delegates=new string [] { "Delegate" }, Events=new Type [] { typeof (NSTextStorageDelegate)})]
	partial interface NSTextStorage : NSSecureCoding {
#if MONOMAC && !XAMCORE_4_0
		[Export ("initWithString:")]
		IntPtr Constructor (string str);
#endif

		[Export ("layoutManagers")]
#if MONOMAC || XAMCORE_4_0
		NSLayoutManager [] LayoutManagers { get; }
#else
		NSObject [] LayoutManagers { get; }
#endif

		[Export ("addLayoutManager:")]
		[PostGet ("LayoutManagers")]
		void AddLayoutManager (NSLayoutManager aLayoutManager);

		[Export ("removeLayoutManager:")]
		[PostGet ("LayoutManagers")]
		void RemoveLayoutManager (NSLayoutManager aLayoutManager);

		[Export ("editedMask")]
#if MONOMAC && !XAMCORE_4_0
		NSTextStorageEditedFlags EditedMask {
#else
		NSTextStorageEditActions EditedMask {
#endif
			get;
#if !XAMCORE_4_0 && !MONOMAC && !__MACCATALYST__
			[NotImplemented] set;
#endif
		}

		[Export ("editedRange")]
		NSRange EditedRange {
			get;
#if !XAMCORE_3_0 && !MONOMAC && !__MACCATALYST__
			[NotImplemented] set;
#endif
		}

		[Export ("changeInLength")]
		nint ChangeInLength {
			get;
#if !XAMCORE_3_0 && !MONOMAC && !__MACCATALYST__
			[NotImplemented] set;
#endif
		}

		[NullAllowed]
		[Export ("delegate", ArgumentSemantic.Assign)]
		NSObject WeakDelegate { get; set; }

		[Wrap ("WeakDelegate")]
		INSTextStorageDelegate Delegate { get; set; }

		[Export ("edited:range:changeInLength:")]
#if MONOMAC && !XAMCORE_4_0
		void Edited (nuint editedMask, NSRange editedRange, nint delta);
#else
		void Edited (NSTextStorageEditActions editedMask, NSRange editedRange, nint delta);
#endif

		[Export ("processEditing")]
		void ProcessEditing ();

		[Export ("fixesAttributesLazily")]
		bool FixesAttributesLazily { get; }

		[Export ("invalidateAttributesInRange:")]
		void InvalidateAttributes (NSRange range);

		[Export ("ensureAttributesAreFixedInRange:")]
		void EnsureAttributesAreFixed (NSRange range);

		[iOS (7,0)]
		[Notification, Field ("NSTextStorageWillProcessEditingNotification")]
#if !MONOMAC || XAMCORE_4_0
		[Internal]
#endif
		NSString WillProcessEditingNotification { get; }

		[iOS (7,0)]
		[Notification, Field ("NSTextStorageDidProcessEditingNotification")]
#if !MONOMAC || XAMCORE_4_0
		[Internal]
#endif
		NSString DidProcessEditingNotification { get; }
	}

	interface INSTextStorageDelegate {}

	[NoWatch]
	[MacCatalyst (13,0)]
	[Model]
	[BaseType (typeof (NSObject))]
	[Protocol]
	partial interface NSTextStorageDelegate {
		[NoiOS][NoTV][NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use WillProcessEditing instead.")]
		[Export ("textStorageWillProcessEditing:")]
		void TextStorageWillProcessEditing (NSNotification notification);

		[NoiOS][NoTV][NoMacCatalyst]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use DidProcessEditing instead.")]
		[Export ("textStorageDidProcessEditing:")]
		void TextStorageDidProcessEditing (NSNotification notification);

		[Mac (10,11)]
		[Export ("textStorage:willProcessEditing:range:changeInLength:")][EventArgs ("NSTextStorage")]
		void WillProcessEditing (NSTextStorage textStorage, NSTextStorageEditActions editedMask, NSRange editedRange, nint delta);

		[Mac (10,11)]
		[Export ("textStorage:didProcessEditing:range:changeInLength:")][EventArgs ("NSTextStorage")]
		void DidProcessEditing (NSTextStorage textStorage, NSTextStorageEditActions editedMask, NSRange editedRange, nint delta);
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[MacCatalyst (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutAnchor : NSCopying, INSCopying
	{
		[Static]
		[Export ("layoutAnchorWithEdges:")]
		NSCollectionLayoutAnchor Create (NSDirectionalRectEdge edges);

		[Static]
		[Export ("layoutAnchorWithEdges:absoluteOffset:")]
		NSCollectionLayoutAnchor CreateFromAbsoluteOffset (NSDirectionalRectEdge edges, CGPoint absoluteOffset);

		[Static]
		[Export ("layoutAnchorWithEdges:fractionalOffset:")]
		NSCollectionLayoutAnchor CreateFromFractionalOffset (NSDirectionalRectEdge edges, CGPoint fractionalOffset);

		[Export ("edges")]
		NSDirectionalRectEdge Edges { get; }

		[Export ("offset")]
		CGPoint Offset { get; }

		[Export ("isAbsoluteOffset")]
		bool IsAbsoluteOffset { get; }

		[Export ("isFractionalOffset")]
		bool IsFractionalOffset { get; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[MacCatalyst (13, 0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutDimension : NSCopying
	{
		[Static]
		[Export ("fractionalWidthDimension:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutDimension CreateFractionalWidthDimension (nfloat fractionalWidth);
#else
		NSCollectionLayoutDimension CreateFractionalWidth (nfloat fractionalWidth);
#endif

		[Static]
		[Export ("fractionalHeightDimension:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutDimension CreateFractionalHeightDimension (nfloat fractionalHeight);
#else
		NSCollectionLayoutDimension CreateFractionalHeight (nfloat fractionalHeight);
#endif

		[Static]
		[Export ("absoluteDimension:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutDimension CreateAbsoluteDimension (nfloat absoluteDimension);
#else
		NSCollectionLayoutDimension CreateAbsolute (nfloat absoluteDimension);
#endif

		[Static]
		[Export ("estimatedDimension:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutDimension CreateEstimatedDimension (nfloat estimatedDimension);
#else
		NSCollectionLayoutDimension CreateEstimated (nfloat estimatedDimension);
#endif

		[Export ("isFractionalWidth")]
		bool IsFractionalWidth { get; }

		[Export ("isFractionalHeight")]
		bool IsFractionalHeight { get; }

		[Export ("isAbsolute")]
		bool IsAbsolute { get; }

		[Export ("isEstimated")]
		bool IsEstimated { get; }

		[Export ("dimension")]
		nfloat Dimension { get; }
	}


	[NoWatch, TV (13,0), iOS (13,0)]
	[MacCatalyst (13, 0)]
	[Mac (10,15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutSize : NSCopying
	{
		[Static]
		[Export ("sizeWithWidthDimension:heightDimension:")]
		NSCollectionLayoutSize Create (NSCollectionLayoutDimension width, NSCollectionLayoutDimension height);

		[Export ("widthDimension")]
		NSCollectionLayoutDimension WidthDimension { get; }

		[Export ("heightDimension")]
		NSCollectionLayoutDimension HeightDimension { get; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[MacCatalyst (13, 0)]
	[Mac (10,15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutSpacing : NSCopying
	{
		[Static]
		[Export ("flexibleSpacing:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutSpacing CreateFlexibleSpacing (nfloat flexibleSpacing);
#else
		NSCollectionLayoutSpacing CreateFlexible (nfloat flexibleSpacing);
#endif

		[Static]
		[Export ("fixedSpacing:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutSpacing CreateFixedSpacing (nfloat fixedSpacing);
#else
		NSCollectionLayoutSpacing CreateFixed (nfloat fixedSpacing);
#endif

		[Export ("spacing")]
		nfloat Spacing { get; }

		[Export ("isFlexibleSpacing")]
		bool IsFlexibleSpacing { get; }

		[Export ("isFixedSpacing")]
		bool IsFixedSpacing { get; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[MacCatalyst (13, 0)]
	[Mac (10,15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutEdgeSpacing : NSCopying
	{
		[Static]
		[Export ("spacingForLeading:top:trailing:bottom:")]
#if MONOMAC && !XAMCORE_4_0
		NSCollectionLayoutEdgeSpacing CreateSpacing ([NullAllowed] NSCollectionLayoutSpacing leading, [NullAllowed] NSCollectionLayoutSpacing top, [NullAllowed] NSCollectionLayoutSpacing trailing, [NullAllowed] NSCollectionLayoutSpacing bottom);
#else
		NSCollectionLayoutEdgeSpacing Create ([NullAllowed] NSCollectionLayoutSpacing leading, [NullAllowed] NSCollectionLayoutSpacing top, [NullAllowed] NSCollectionLayoutSpacing trailing, [NullAllowed] NSCollectionLayoutSpacing bottom);
#endif

		[NullAllowed, Export ("leading")]
		NSCollectionLayoutSpacing Leading { get; }

		[NullAllowed, Export ("top")]
		NSCollectionLayoutSpacing Top { get; }

		[NullAllowed, Export ("trailing")]
		NSCollectionLayoutSpacing Trailing { get; }

		[NullAllowed, Export ("bottom")]
		NSCollectionLayoutSpacing Bottom { get; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[MacCatalyst (13, 0)]
	[Mac (10,15)]
	[BaseType (typeof (NSCollectionLayoutItem))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutSupplementaryItem : NSCopying
	{
		[Static]
		[Export ("supplementaryItemWithLayoutSize:elementKind:containerAnchor:")]
		NSCollectionLayoutSupplementaryItem Create (NSCollectionLayoutSize layoutSize, string elementKind, NSCollectionLayoutAnchor containerAnchor);

		[Static]
		[Export ("supplementaryItemWithLayoutSize:elementKind:containerAnchor:itemAnchor:")]
		NSCollectionLayoutSupplementaryItem Create (NSCollectionLayoutSize layoutSize, string elementKind, NSCollectionLayoutAnchor containerAnchor, NSCollectionLayoutAnchor itemAnchor);

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("elementKind")]
		string ElementKind { get; }

		[Export ("containerAnchor")]
		NSCollectionLayoutAnchor ContainerAnchor { get; }

		[NullAllowed, Export ("itemAnchor")]
		NSCollectionLayoutAnchor ItemAnchor { get; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[MacCatalyst (13, 0)]
	[Mac (10,15)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutItem : NSCopying
	{
		[Static]
		[Export ("itemWithLayoutSize:")]
		NSCollectionLayoutItem Create (NSCollectionLayoutSize layoutSize);

		[Static]
		[Export ("itemWithLayoutSize:supplementaryItems:")]
		NSCollectionLayoutItem Create (NSCollectionLayoutSize layoutSize, params NSCollectionLayoutSupplementaryItem[] supplementaryItems);

		[Export ("contentInsets", ArgumentSemantic.Assign)]
		NSDirectionalEdgeInsets ContentInsets { get; set; }

		[NullAllowed, Export ("edgeSpacing", ArgumentSemantic.Copy)]
		NSCollectionLayoutEdgeSpacing EdgeSpacing { get; set; }

		[Export ("layoutSize")]
		NSCollectionLayoutSize LayoutSize { get; }

		[Export ("supplementaryItems")]
		NSCollectionLayoutSupplementaryItem[] SupplementaryItems { get; }
	}

	[NoWatch, TV (13,0), iOS (13,0)]
	[MacCatalyst (13, 0)]
	[Mac (10,15)]
	[BaseType (typeof (NSCollectionLayoutSupplementaryItem))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutBoundarySupplementaryItem : NSCopying
	{
		[Static]
		[Export ("boundarySupplementaryItemWithLayoutSize:elementKind:alignment:")]
		NSCollectionLayoutBoundarySupplementaryItem Create (NSCollectionLayoutSize layoutSize, string elementKind, NSRectAlignment alignment);

		[Static]
		[Export ("boundarySupplementaryItemWithLayoutSize:elementKind:alignment:absoluteOffset:")]
		NSCollectionLayoutBoundarySupplementaryItem Create (NSCollectionLayoutSize layoutSize, string elementKind, NSRectAlignment alignment, CGPoint absoluteOffset);

		[Export ("extendsBoundary")]
		bool ExtendsBoundary { get; set; }

		[Export ("pinToVisibleBounds")]
		bool PinToVisibleBounds { get; set; }

		[Export ("alignment")]
		NSRectAlignment Alignment { get; }

		[Export ("offset")]
		CGPoint Offset { get; }
	}

	[MacCatalyst (13, 0)]
	[NoWatch, TV (13,0), iOS (13,0)]
	[Mac (10,15)]
	[BaseType (typeof (NSCollectionLayoutItem))]
	[DisableDefaultCtor]
	interface NSCollectionLayoutDecorationItem : NSCopying
	{
		[Static]
		[Export ("backgroundDecorationItemWithElementKind:")]
		NSCollectionLayoutDecorationItem Create (string elementKind);

		[Export ("zIndex")]
		nint ZIndex { get; set; }

		[Export ("elementKind")]
		string ElementKind { get; }
	}

	[iOS (9,0), Watch (2,0)]
	[MacCatalyst (13,0)]
	[Mac (10,11)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor] // - (instancetype)init NS_UNAVAILABLE;
	interface NSDataAsset : NSCopying
	{
		[Export ("initWithName:")]
		IntPtr Constructor (string name);

		[Export ("initWithName:bundle:")]
		[DesignatedInitializer]
		IntPtr Constructor (string name, NSBundle bundle);

		[Export ("name")]
		string Name { get; }

		[Export ("data", ArgumentSemantic.Copy)]
		NSData Data { get; }

		[Export ("typeIdentifier")] // Uniform Type Identifier
		NSString TypeIdentifier { get; }
	}

	[MacCatalyst (13,0)]
	[Watch (6,0)]
	[BaseType (typeof (NSObject))]
	[DesignatedDefaultCtor]
	interface NSShadow : NSSecureCoding, NSCopying {
		[NoiOS][NoMacCatalyst][NoTV][NoWatch]
		[Export ("set")]
		void Set ();

		[Export ("shadowOffset", ArgumentSemantic.Assign)]
		CGSize ShadowOffset { get; set; }

		[Export ("shadowBlurRadius", ArgumentSemantic.Assign)]
		nfloat ShadowBlurRadius { get; set;  }

#if MONOMAC
		[Export ("shadowColor", ArgumentSemantic.Copy)]
#else
		[Export ("shadowColor", ArgumentSemantic.Retain), NullAllowed]
#endif
		NSColor ShadowColor { get; set;  }
	}

	[iOS (7,0)]
	[MacCatalyst (13,0)]
	[BaseType (typeof (NSObject))]
	interface NSTextTab : NSSecureCoding, NSCopying {
		[DesignatedInitializer]
		[Export ("initWithTextAlignment:location:options:")]
		[PostGet ("Options")]
		IntPtr Constructor (TextAlignment alignment, nfloat location, NSDictionary options);

		[NoiOS][NoMacCatalyst][NoTV][NoWatch]
		[Export ("initWithType:location:")]
		IntPtr Constructor (NSTextTabType type, nfloat location);

		[Export ("alignment")]
		TextAlignment Alignment { get; }

		[Export ("options")]
		NSDictionary Options { get; }

		[Export ("location")]
		nfloat Location { get; }

		[NoiOS][NoMacCatalyst][NoTV][NoWatch]
		[Export ("tabStopType")]
		NSTextTabType TabStopType { get; }

		[Mac (10,11)]
		[Static]
		[Export ("columnTerminatorsForLocale:")]
		NSCharacterSet GetColumnTerminators ([NullAllowed] NSLocale locale);

		[Field ("NSTabColumnTerminatorsAttributeName")]
		NSString ColumnTerminatorsAttributeName { get; }
	}

	[NoWatch]
	[MacCatalyst (13,0)]
	[Protocol]
	// no [Model] since it's not exposed in any API
	// only NSTextContainer conforms to it but it's only queried by iOS itself
	interface NSTextLayoutOrientationProvider {
		[Abstract]
		[Export ("layoutOrientation")]
		NSTextLayoutOrientation LayoutOrientation {
			get;
#if !XAMCORE_3_0 && !MONOMAC
			[NotImplemented] set;
#endif
		}
	}

	[NoWatch]
	[iOS (7,0)]
	[BaseType (typeof (NSObject))]
	partial interface NSTextContainer : NSTextLayoutOrientationProvider, NSSecureCoding {
		[NoMac]
		[DesignatedInitializer]
		[Export ("initWithSize:")]
		IntPtr Constructor (CGSize size);

		[NoiOS][NoMacCatalyst][NoTV]
		[Export ("initWithContainerSize:"), Internal]
		[Sealed]
		IntPtr InitWithContainerSize (CGSize size);

		[NoiOS][NoMacCatalyst][NoTV]
		[Mac (10,11)]
		[Export ("initWithSize:"), Internal]
		[Sealed]
		IntPtr InitWithSize (CGSize size);

		[NullAllowed] // by default this property is null
		[Export ("layoutManager", ArgumentSemantic.Assign)]
		NSLayoutManager LayoutManager { get; set; }

		[Mac (10,11)]
		[Export ("size")]
		CGSize Size { get; set; }

		[Mac (10,11)]
		[Export ("exclusionPaths", ArgumentSemantic.Copy)]
		BezierPath [] ExclusionPaths { get; set; }

		[Mac (10,11)]
		[Export ("lineBreakMode")]
		LineBreakMode LineBreakMode { get; set; }

		[Export ("lineFragmentPadding")]
		nfloat LineFragmentPadding { get; set; }

		[Mac (10,11)]
		[Export ("maximumNumberOfLines")]
		nuint MaximumNumberOfLines { get; set; }

		[Mac (10,11)]
		[Export ("lineFragmentRectForProposedRect:atIndex:writingDirection:remainingRect:")]
#if MONOMAC && !XAMCORE_4_0
		CGRect GetLineFragmentRect (CGRect proposedRect, nuint characterIndex, NSWritingDirection baseWritingDirection, ref CGRect remainingRect);
#else
		CGRect GetLineFragmentRect (CGRect proposedRect, nuint characterIndex, NSWritingDirection baseWritingDirection, out CGRect remainingRect);
#endif

		[Export ("widthTracksTextView")]
		bool WidthTracksTextView { get; set; }

		[Export ("heightTracksTextView")]
		bool HeightTracksTextView { get; set; }

		[iOS (9,0)]
		[Export ("replaceLayoutManager:")]
		void ReplaceLayoutManager (NSLayoutManager newLayoutManager);

		[iOS (9,0)]
		[Export ("simpleRectangularTextContainer")]
		bool IsSimpleRectangularTextContainer { [Bind ("isSimpleRectangularTextContainer")] get; }

		[NoiOS][NoMacCatalyst][NoTV]
		[Deprecated (PlatformName.MacOSX, 10, 11)]
		[Export ("containsPoint:")]
		bool ContainsPoint (CGPoint point);

		[NoiOS][NoMacCatalyst][NoTV]
		[Export ("textView", ArgumentSemantic.Weak)]
		NSTextView TextView { get; set; }

		[NoiOS][NoMacCatalyst][NoTV]
		[Availability (Deprecated = Platform.Mac_10_11, Message = "Use Size instead.")]
		[Export ("containerSize")]
		CGSize ContainerSize { get; set; }
	}

	[ThreadSafe]
	[Category, BaseType (typeof (NSString))]
	interface NSExtendedStringDrawing {
		[iOS (7,0)]
		[Mac (10,11)]
		[Export ("drawWithRect:options:attributes:context:")]
		void WeakDrawString (CGRect rect, NSStringDrawingOptions options, [NullAllowed] NSDictionary attributes, [NullAllowed] NSStringDrawingContext context);

		[iOS (7,0)]
		[Mac (10,11)]
		[Wrap ("WeakDrawString (This, rect, options, attributes.GetDictionary (), context)")]
		void DrawString (CGRect rect, NSStringDrawingOptions options, StringAttributes attributes, [NullAllowed] NSStringDrawingContext context);

		[iOS (7,0)]
		[Mac (10,11)]
		[Export ("boundingRectWithSize:options:attributes:context:")]
		CGRect WeakGetBoundingRect (CGSize size, NSStringDrawingOptions options, [NullAllowed] NSDictionary attributes, [NullAllowed] NSStringDrawingContext context);

		[iOS (7,0)]
		[Mac (10,11)]
		[Wrap ("WeakGetBoundingRect (This, size, options, attributes.GetDictionary (), context)")]
		CGRect GetBoundingRect (CGSize size, NSStringDrawingOptions options, StringAttributes attributes, [NullAllowed] NSStringDrawingContext context);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface NSTextLayoutManagerDelegate
	{
		[Export ("textLayoutManager:textLayoutFragmentForLocation:inTextElement:")]
		NSTextLayoutFragment GetTextLayoutFragment (NSTextLayoutManager textLayoutManager, INSTextLocation location, NSTextElement textElement);

		[Export ("textLayoutManager:shouldBreakLineBeforeLocation:hyphenating:")]
		bool ShouldBreakLineBeforeLocation (NSTextLayoutManager textLayoutManager, INSTextLocation location, bool hyphenating);

		[Export ("textLayoutManager:renderingAttributesForLink:atLocation:defaultAttributes:")]
		[return: NullAllowed]
		NSDictionary<NSString, NSObject> GetRenderingAttributes (NSTextLayoutManager textLayoutManager, NSObject link, INSTextLocation location, NSDictionary<NSString, NSObject> renderingAttributes);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextLayoutManagerSegmentType : long {
		Standard = 0,
		Selection = 1,
		Highlight = 2,
	}

	[TV (15,0) ,NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Flags]
	[Native]
	public enum NSTextLayoutManagerSegmentOptions : ulong {
		None = 0x0,
		RangeNotRequired = (1uL << 0),
		MiddleFragmentsExcluded = (1uL << 1),
		HeadSegmentExtended = (1uL << 2),
		TailSegmentExtended = (1uL << 3),
		UpstreamAffinity = (1uL << 4),
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Flags]
	[Native]
	public enum NSTextLayoutFragmentEnumerationOptions : ulong {
		None = 0x0,
		Reverse = (1uL << 0),
		EstimatesSize = (1uL << 1),
		EnsuresLayout = (1uL << 2),
		EnsuresExtraLineFragment = (1uL << 3),
	}

	interface INSTextLayoutManagerDelegate {}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	delegate bool NSTextLayoutManagerEnumerateRenderingAttributesDelegate (NSTextLayoutManager textLayoutManager, NSDictionary<NSString, NSObject> attributes, NSTextRange textRange);

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	delegate bool NSTextLayoutManagerEnumerateTextSegmentsDelegate (NSTextRange textSegmentRange, CGRect textSegmentFrame, nfloat baselinePosition, NSTextContainer textContainer);

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[DesignatedDefaultCtor]
	[BaseType (typeof (NSObject))]
	interface NSTextLayoutManager : NSSecureCoding, NSTextSelectionDataSource
	{
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		INSTextLayoutManagerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("usesFontLeading")]
		bool UsesFontLeading { get; set; }

		[Export ("limitsLayoutForSuspiciousContents")]
		bool LimitsLayoutForSuspiciousContents { get; set; }

		[Export ("usesHyphenation")]
		bool UsesHyphenation { get; set; }

		[NullAllowed, Export ("textContentManager", ArgumentSemantic.Weak)]
		NSTextContentManager TextContentManager { get; }

		[Export ("replaceTextContentManager:")]
		void Replace (NSTextContentManager textContentManager);

		[NullAllowed, Export ("textContainer", ArgumentSemantic.Strong)]
		NSTextContainer TextContainer { get; set; }

		[Export ("usageBoundsForTextContainer")]
		CGRect UsageBoundsForTextContainer { get; }

		[Export ("textViewportLayoutController", ArgumentSemantic.Strong)]
		NSTextViewportLayoutController TextViewportLayoutController { get; }

		[NullAllowed, Export ("layoutQueue", ArgumentSemantic.Strong)]
		NSOperationQueue LayoutQueue { get; set; }

		[Export ("ensureLayoutForRange:")]
		void EnsureLayout (NSTextRange range);

		[Export ("ensureLayoutForBounds:")]
		void EnsureLayout (CGRect bounds);

		[Export ("invalidateLayoutForRange:")]
		void InvalidateLayout (NSTextRange range);

		[Export ("textLayoutFragmentForPosition:")]
		[return: NullAllowed]
		NSTextLayoutFragment GetTextLayoutFragment (CGPoint position);

		[Export ("textLayoutFragmentForLocation:")]
		[return: NullAllowed]
		NSTextLayoutFragment GetTextLayoutFragment (INSTextLocation location);

		[Export ("enumerateTextLayoutFragmentsFromLocation:options:usingBlock:")]
		[return: NullAllowed]
		INSTextLocation EnumerateTextLayoutFragments ([NullAllowed] INSTextLocation location, NSTextLayoutFragmentEnumerationOptions options, Func<NSTextLayoutFragment, bool> handler);

		[Export ("textSelections", ArgumentSemantic.Strong)]
		NSTextSelection[] TextSelections { get; set; }

		[Export ("textSelectionNavigation", ArgumentSemantic.Strong)]
		NSTextSelectionNavigation TextSelectionNavigation { get; set; }

		[Export ("enumerateRenderingAttributesFromLocation:reverse:usingBlock:")]
		void EnumerateRenderingAttributes (INSTextLocation location, bool reverse, NSTextLayoutManagerEnumerateRenderingAttributesDelegate handler);

		[Export ("setRenderingAttributes:forTextRange:")]
		void SetRenderingAttributes (NSDictionary<NSString, NSObject> renderingAttributes, NSTextRange textRange);

		[Export ("addRenderingAttribute:value:forTextRange:")]
		void AddRenderingAttribute (string renderingAttribute, [NullAllowed] NSObject value, NSTextRange textRange);

		[Export ("removeRenderingAttribute:forTextRange:")]
		void RemoveRenderingAttribute (string renderingAttribute, NSTextRange textRange);

		[Export ("invalidateRenderingAttributesForTextRange:")]
		void InvalidateRenderingAttributes (NSTextRange textRange);

		[NullAllowed, Export ("renderingAttributesValidator", ArgumentSemantic.Copy)]
		Action<NSTextLayoutManager, NSTextLayoutFragment> RenderingAttributesValidator { get; set; }

		[Static]
		[Export ("linkRenderingAttributes")]
		NSDictionary<NSString, NSObject> LinkRenderingAttributes { get; }

		[Export ("renderingAttributesForLink:atLocation:")]
		NSDictionary<NSString, NSObject> GetRenderingAttributes (NSObject link, INSTextLocation location);

		[Export ("enumerateTextSegmentsInRange:type:options:usingBlock:")]
		void EnumerateTextSegments (NSTextRange textRange, NSTextLayoutManagerSegmentType type, NSTextLayoutManagerSegmentOptions options, NSTextLayoutManagerEnumerateTextSegmentsDelegate  handler);

		[Export ("replaceContentsInRange:withTextElements:")]
		void ReplaceContents (NSTextRange range, NSTextElement[] textElements);

		[Export ("replaceContentsInRange:withAttributedString:")]
		void ReplaceContents (NSTextRange range, NSAttributedString attributedString);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Flags]
	[Native]
	public enum NSTextContentManagerEnumerationOptions : ulong
	{
		None = 0x0,
		Reverse = (1uL << 0),
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface NSTextContentManagerDelegate
	{
		[Export ("textContentManager:textElementAtLocation:")]
		[return: NullAllowed]
		NSTextElement GetTextContentManager (NSTextContentManager textContentManager, INSTextLocation location);

		[Export ("textContentManager:shouldEnumerateTextElement:options:")]
		bool ShouldEnumerateTextElement (NSTextContentManager textContentManager, NSTextElement textElement, NSTextContentManagerEnumerationOptions options);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Protocol]
	interface NSTextElementProvider
	{
		[Abstract]
		[Export ("documentRange", ArgumentSemantic.Strong)]
		NSTextRange DocumentRange { get; }

		[Abstract]
		[Export ("enumerateTextElementsFromLocation:options:usingBlock:")]
		[return: NullAllowed]
		INSTextLocation EnumerateTextElements ([NullAllowed] INSTextLocation textLocation, NSTextContentManagerEnumerationOptions options, Func<NSTextElement, bool> handler);

		[Abstract]
		[Export ("replaceContentsInRange:withTextElements:")]
		void ReplaceContents (NSTextRange range, [NullAllowed] NSTextElement[] textElements);

		[Abstract]
		[Export ("synchronizeToBackingStore:")]
		void Synchronize ([NullAllowed] Action<NSError> completionHandler);

		[Export ("locationFromLocation:withOffset:")]
		[return: NullAllowed]
		INSTextLocation GetLocation (INSTextLocation location, nint offset);

		[Export ("offsetFromLocation:toLocation:")]
		nint GetOffset (INSTextLocation from, INSTextLocation to);

		[Export ("adjustedRangeFromRange:forEditingTextSelection:")]
		[return: NullAllowed]
		NSTextRange AdjustedRange (NSTextRange textRange, bool forEditingTextSelection);
	}

	interface INSTextContentManagerDelegate {}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextContentManager : NSTextElementProvider, NSSecureCoding
	{
		[DesignatedInitializer]
		[Export ("init")]
		IntPtr Constructor ();

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		INSTextContentManagerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[Export ("textLayoutManagers", ArgumentSemantic.Copy)]
		NSTextLayoutManager[] TextLayoutManagers { get; }

		[Export ("addTextLayoutManager:")]
		void Add (NSTextLayoutManager textLayoutManager);

		[Export ("removeTextLayoutManager:")]
		void Remove (NSTextLayoutManager textLayoutManager);

		[NullAllowed, Export ("primaryTextLayoutManager", ArgumentSemantic.Strong)]
		NSTextLayoutManager PrimaryTextLayoutManager { get; set; }

		[Async]
		[Export ("synchronizeTextLayoutManagers:")]
		void SynchronizeTextLayoutManagers ([NullAllowed] Action<NSError> completionHandler);

		[Export ("textElementsForRange:")]
		NSTextElement[] GetTextElements (NSTextRange range);

		[Export ("hasEditingTransaction")]
		bool HasEditingTransaction { get; }

		[Async]
		[Export ("performEditingTransactionUsingBlock:")]
		void PerformEditingTransaction (Action transaction);

		[Export ("recordEditActionInRange:newTextRange:")]
		void RecordEditAction (NSTextRange originalTextRange, NSTextRange newTextRange);

		[Export ("automaticallySynchronizesTextLayoutManagers")]
		bool AutomaticallySynchronizesTextLayoutManagers { get; set; }

		[Export ("automaticallySynchronizesToBackingStore")]
		bool AutomaticallySynchronizesToBackingStore { get; set; }
	}

	interface INSTextLocation {}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Protocol]
	interface NSTextLocation
	{
		[Abstract]
		[Export ("compare:")]
		NSComparisonResult Compare (INSTextLocation location);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	interface NSTextElement
	{
		[Export ("initWithTextContentManager:")]
		[DesignatedInitializer]
		IntPtr Constructor ([NullAllowed] NSTextContentManager textContentManager);

		[NullAllowed, Export ("textContentManager", ArgumentSemantic.Weak)]
		NSTextContentManager TextContentManager { get; set; }

		[NullAllowed, Export ("elementRange", ArgumentSemantic.Strong)]
		NSTextRange ElementRange { get; set; }
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSTextElement))]
	interface NSTextParagraph
	{
		[Export ("initWithAttributedString:")]
		[DesignatedInitializer]
		IntPtr Constructor ([NullAllowed] NSAttributedString attributedString);

		[Export ("initWithTextContentManager:")]
		[DesignatedInitializer]
		IntPtr Constructor ([NullAllowed] NSTextContentManager textContentManager);

		[Export ("attributedString", ArgumentSemantic.Strong)]
		NSAttributedString AttributedString { get; }

		[NullAllowed, Export ("paragraphContentRange", ArgumentSemantic.Strong)]
		NSTextRange ParagraphContentRange { get; }

		[NullAllowed, Export ("paragraphSeparatorRange", ArgumentSemantic.Strong)]
		NSTextRange ParagraphSeparatorRange { get; }
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextLineFragment : NSSecureCoding
	{
		[Export ("initWithAttributedString:range:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSAttributedString attributedString, NSRange range);

		[Export ("initWithString:attributes:range:")]
		IntPtr Constructor (string @string, NSDictionary<NSString, NSObject> attributes, NSRange range);

		[Export ("attributedString", ArgumentSemantic.Strong)]
		NSAttributedString AttributedString { get; }

		[Export ("characterRange")]
		NSRange CharacterRange { get; }

		[Export ("typographicBounds")]
		CGRect TypographicBounds { get; }

		[Export ("glyphOrigin")]
		CGPoint GlyphOrigin { get; }

		[Export ("drawAtPoint:inContext:")]
		void Draw (CGPoint point, CGContext context);

		[Export ("locationForCharacterAtIndex:")]
		CGPoint GetLocation (nint characterIndex);

		[Export ("characterIndexForPoint:")]
		nint GetCharacterIndex (CGPoint point);

		[Export ("fractionOfDistanceThroughGlyphForPoint:")]
		nfloat GetFractionOfDistanceThroughGlyph (CGPoint point);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextLayoutFragmentState : ulong {
		None = 0,
		EstimatedUsageBounds = 1,
		CalculatedUsageBounds = 2,
		LayoutAvailable = 3,
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextAttachmentViewProvider
	{
		[Export ("initWithTextAttachment:parentView:textLayoutManager:location:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSTextAttachment textAttachment, [NullAllowed] View parentView, [NullAllowed] NSTextLayoutManager textLayoutManager, INSTextLocation location);

		[NullAllowed, Export ("textAttachment", ArgumentSemantic.Weak)]
		NSTextAttachment TextAttachment { get; }

		[NullAllowed, Export ("textLayoutManager", ArgumentSemantic.Weak)]
		NSTextLayoutManager TextLayoutManager { get; }

		[Export ("location", ArgumentSemantic.Strong)]
		INSTextLocation Location { get; }

		[NullAllowed, Export ("view", ArgumentSemantic.Strong)]
		View View { get; set; }

		[Export ("loadView")]
		void LoadView ();

		[Export ("tracksTextAttachmentViewBounds")]
		bool TracksTextAttachmentViewBounds { get; set; }

		[Export ("attachmentBoundsForAttributes:location:textContainer:proposedLineFragment:position:")]
		CGRect GetAttachmentBounds (NSDictionary<NSString, NSObject> attributes, INSTextLocation location, [NullAllowed] NSTextContainer textContainer, CGRect proposedLineFragment, CGPoint position);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextLayoutFragment : NSSecureCoding
	{
		[Export ("initWithTextElement:range:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSTextElement textElement, [NullAllowed] NSTextRange rangeInElement);

		[NullAllowed, Export ("textLayoutManager", ArgumentSemantic.Weak)]
		NSTextLayoutManager TextLayoutManager { get; }

		[NullAllowed, Export ("textElement", ArgumentSemantic.Weak)]
		NSTextElement TextElement { get; }

		[Export ("rangeInElement", ArgumentSemantic.Strong)]
		NSTextRange RangeInElement { get; }

		[Export ("textLineFragments", ArgumentSemantic.Copy)]
		NSTextLineFragment[] TextLineFragments { get; }

		[NullAllowed, Export ("layoutQueue", ArgumentSemantic.Strong)]
		NSOperationQueue LayoutQueue { get; set; }

		[Export ("state")]
		NSTextLayoutFragmentState State { get; }

		[Export ("invalidateLayout")]
		void InvalidateLayout ();

		[Export ("layoutFragmentFrame")]
		CGRect LayoutFragmentFrame { get; }

		[Export ("renderingSurfaceBounds")]
		CGRect RenderingSurfaceBounds { get; }

		[Export ("leadingPadding")]
		nfloat LeadingPadding { get; }

		[Export ("trailingPadding")]
		nfloat TrailingPadding { get; }

		[Export ("topMargin")]
		nfloat TopMargin { get; }

		[Export ("bottomMargin")]
		nfloat BottomMargin { get; }

		[Export ("drawAtPoint:inContext:")]
		void Draw (CGPoint point, CGContext context);

		[Export ("textAttachmentViewProviders", ArgumentSemantic.Copy)]
		NSTextAttachmentViewProvider[] TextAttachmentViewProviders { get; }

		[Export ("frameForTextAttachmentAtLocation:")]
		CGRect GetFrameForTextAttachment (INSTextLocation location);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextRange
	{
		[Export ("initWithLocation:endLocation:")]
		[DesignatedInitializer]
		IntPtr Constructor (INSTextLocation location, [NullAllowed] INSTextLocation endLocation);

		[Export ("initWithLocation:")]
		IntPtr Constructor (INSTextLocation location);

		[Export ("empty")]
		bool Empty { [Bind ("isEmpty")] get; }

		[Export ("location", ArgumentSemantic.Strong)]
		INSTextLocation Location { get; }

		[Export ("endLocation", ArgumentSemantic.Strong)]
		INSTextLocation EndLocation { get; }

		[Export ("isEqualToTextRange:")]
		bool IsEqual (NSTextRange textRange);

		[Export ("containsLocation:")]
		bool Contains (INSTextLocation location);

		[Export ("containsRange:")]
		bool Contains (NSTextRange textRange);

		[Export ("intersectsWithTextRange:")]
		bool Intersects (NSTextRange textRange);

		[Export ("textRangeByIntersectingWithTextRange:")]
		[return: NullAllowed]
		NSTextRange GetTextRangeByIntersecting (NSTextRange textRange);

		[Export ("textRangeByFormingUnionWithTextRange:")]
		NSTextRange GetTextRangeByFormingUnion (NSTextRange textRange);
	}
	
	interface INSTextViewportLayoutControllerDelegate {}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface NSTextViewportLayoutControllerDelegate
	{
		[Abstract]
		[Export ("viewportBoundsForTextViewportLayoutController:")]
		CGRect GetViewportBounds (NSTextViewportLayoutController textViewportLayoutController);

		[Abstract]
		[Export ("textViewportLayoutController:configureRenderingSurfaceForTextLayoutFragment:")]
		void ConfigureRenderingSurface (NSTextViewportLayoutController textViewportLayoutController, NSTextLayoutFragment textLayoutFragment);

		[Export ("textViewportLayoutControllerWillLayout:")]
		void WillLayout (NSTextViewportLayoutController textViewportLayoutController);

		[Export ("textViewportLayoutControllerDidLayout:")]
		void DidLayout (NSTextViewportLayoutController textViewportLayoutController);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextViewportLayoutController
	{
		[Export ("initWithTextLayoutManager:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSTextLayoutManager textLayoutManager);

		[Wrap ("WeakDelegate")]
		[NullAllowed]
		INSTextViewportLayoutControllerDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("textLayoutManager", ArgumentSemantic.Weak)]
		NSTextLayoutManager TextLayoutManager { get; }

		[Export ("viewportBounds")]
		CGRect ViewportBounds { get; }

		[NullAllowed, Export ("viewportRange")]
		NSTextRange ViewportRange { get; }

		[Export ("layoutViewport")]
		void LayoutViewport ();

		[Export ("relocateViewportToTextLocation:")]
		nfloat RelocateViewport (INSTextLocation textLocation);

		[Export ("adjustViewportByVerticalOffset:")]
		void AdjustViewport (nfloat verticalOffset);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextSelectionGranularity : long
	{
		Character,
		Word,
		Paragraph,
		Line,
		Sentence,
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextSelectionAffinity : long {
		Upstream = 0,
		Downstream = 1,
	}


	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextSelection : NSSecureCoding
	{
		[Export ("initWithRanges:affinity:granularity:")]
		[DesignatedInitializer]
		IntPtr Constructor (NSTextRange[] textRanges, NSTextSelectionAffinity affinity, NSTextSelectionGranularity granularity);

		[Export ("initWithRange:affinity:granularity:")]
		IntPtr Constructor (NSTextRange range, NSTextSelectionAffinity affinity, NSTextSelectionGranularity granularity);

		[Export ("initWithLocation:affinity:")]
		IntPtr Constructor (INSTextLocation location, NSTextSelectionAffinity affinity);

		[Export ("textRanges", ArgumentSemantic.Copy)]
		NSTextRange[] TextRanges { get; }

		[Export ("granularity")]
		NSTextSelectionGranularity Granularity { get; }

		[Export ("affinity")]
		NSTextSelectionAffinity Affinity { get; }

		[Export ("transient")]
		bool Transient { [Bind ("isTransient")] get; }

		[Export ("anchorPositionOffset")]
		nfloat AnchorPositionOffset { get; set; }

		[Export ("logical")]
		bool Logical { [Bind ("isLogical")] get; set; }

		[NullAllowed, Export ("secondarySelectionLocation", ArgumentSemantic.Strong)]
		INSTextLocation SecondarySelectionLocation { get; set; }

		[Export ("typingAttributes", ArgumentSemantic.Copy)]
		NSDictionary<NSString, NSObject> TypingAttributes { get; set; }

		[Export ("textSelectionWithTextRanges:")]
		NSTextSelection GetTextSelection (NSTextRange[] textRanges);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	delegate void NSTextSelectionDataSourceEnumerateSubstringsDelegate (NSString substring, NSTextRange substringRange, NSTextRange enclodingRange, out bool stop);

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	delegate void NSTextSelectionDataSourceEnumerateCaretOffsetsDelegate (nfloat caretOffset, INSTextLocation location, bool leadingEdge, out bool stop);

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	delegate void NSTextSelectionDataSourceEnumerateContainerBoundariesDelegate (INSTextLocation location, out bool stop); 

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextSelectionNavigationLayoutOrientation : long {
		Horizontal = 0,
		Vertical = 1,
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextSelectionNavigationWritingDirection : long {
		LeftToRight = 0,
		RightToLeft = 1,
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface NSTextSelectionDataSource
	{
		[Abstract]
		[Export ("documentRange", ArgumentSemantic.Strong)]
		NSTextRange DocumentRange { get; }

		[Abstract]
		[Export ("enumerateSubstringsFromLocation:options:usingBlock:")]
		void EnumerateSubstrings (INSTextLocation location, NSStringEnumerationOptions options, NSTextSelectionDataSourceEnumerateSubstringsDelegate handler); 

		[Abstract]
		[Export ("textRangeForSelectionGranularity:enclosingLocation:")]
		[return: NullAllowed]
		NSTextRange GetTextRange (NSTextSelectionGranularity selectionGranularity, INSTextLocation location);

		[Abstract]
		[Export ("locationFromLocation:withOffset:")]
		[return: NullAllowed]
		INSTextLocation GetLocation (INSTextLocation location, nint offset);

		[Abstract]
		[Export ("offsetFromLocation:toLocation:")]
		nint GetOffsetFromLocation (INSTextLocation from, INSTextLocation to);

		[Abstract]
		[Export ("baseWritingDirectionAtLocation:")]
		NSTextSelectionNavigationWritingDirection GetBaseWritingDirection (INSTextLocation location);

		[Abstract]
		[Export ("enumerateCaretOffsetsInLineFragmentAtLocation:usingBlock:")]
		void EnumerateCaretOffsets (INSTextLocation location, NSTextSelectionDataSourceEnumerateCaretOffsetsDelegate handler);

		[Abstract]
		[Export ("lineFragmentRangeForPoint:inContainerAtLocation:")]
		[return: NullAllowed]
		NSTextRange GetLineFragmentRange (CGPoint point, INSTextLocation location);

		[Export ("enumerateContainerBoundariesFromLocation:reverse:usingBlock:")]
		void EnumerateContainerBoundaries (INSTextLocation location, bool reverse, NSTextSelectionDataSourceEnumerateContainerBoundariesDelegate handler);

		[Export ("textLayoutOrientationAtLocation:")]
		NSTextSelectionNavigationLayoutOrientation GetTextLayoutOrientation (INSTextLocation location);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextSelectionNavigationDirection : long {
		Forward,
		Backward,
		Right,
		Left,
		Up,
		Down,
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Native]
	public enum NSTextSelectionNavigationDestination : long {
		Character,
		Word,
		Line,
		Sentence,
		Paragraph,
		Container,
		Document,
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Flags]
	[Native]
	public enum NSTextSelectionNavigationModifier : ulong {
		Extend = (1uL << 0),
		Visual = (1uL << 1),
		Multiple = (1uL << 2),
	}

	interface INSTextSelectionDataSource {}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSObject))]
	[DisableDefaultCtor]
	interface NSTextSelectionNavigation
	{
		[Export ("initWithDataSource:")]
		[DesignatedInitializer]
		IntPtr Constructor (INSTextSelectionDataSource dataSource);

		[Wrap ("WeakTextSelectionDataSource")]
		[NullAllowed]
		INSTextSelectionDataSource TextSelectionDataSource { get; }

		[NullAllowed, Export ("textSelectionDataSource", ArgumentSemantic.Weak)]
		NSObject WeakTextSelectionDataSource { get; }

		[Export ("allowsNonContiguousRanges")]
		bool AllowsNonContiguousRanges { get; set; }

		[Export ("rotatesCoordinateSystemForLayoutOrientation")]
		bool RotatesCoordinateSystemForLayoutOrientation { get; set; }

		[Export ("flushLayoutCache")]
		void FlushLayoutCache ();

		[Export ("destinationSelectionForTextSelection:direction:destination:extending:confined:")]
		[return: NullAllowed]
		NSTextSelection GetDestinationSelection (NSTextSelection textSelection, NSTextSelectionNavigationDirection direction, NSTextSelectionNavigationDestination destination, bool extending, bool confined);

		[Export ("textSelectionsInteractingAtPoint:inContainerAtLocation:anchors:modifiers:selecting:bounds:")]
		NSTextSelection[] GetTextSelectionsInteracting (CGPoint point, INSTextLocation containerLocation, NSTextSelection[] anchors, NSTextSelectionNavigationModifier modifiers, bool selecting, CGRect bounds);

		[Export ("textSelectionForSelectionGranularity:enclosingTextSelection:")]
		NSTextSelection GetTextSelection (NSTextSelectionGranularity selectionGranularity, NSTextSelection textSelection);

		[Export ("textSelectionForSelectionGranularity:enclosingPoint:inContainerAtLocation:")]
		[return: NullAllowed]
		NSTextSelection GetTextSelection (NSTextSelectionGranularity selectionGranularity, CGPoint point, INSTextLocation location);

		[Export ("resolvedInsertionLocationForTextSelection:writingDirection:")]
		[return: NullAllowed]
		INSTextLocation GetResolvedInsertionLocation (NSTextSelection textSelection, NSTextSelectionNavigationWritingDirection writingDirection);

		[Export ("deletionRangesForTextSelection:direction:destination:allowsDecomposition:")]
		NSTextRange[] GetDeletionRanges (NSTextSelection textSelection, NSTextSelectionNavigationDirection direction, NSTextSelectionNavigationDestination destination, bool allowsDecomposition);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Protocol, Model (AutoGeneratedName = true)]
	[BaseType (typeof (NSObject))]
	interface NSTextContentStorageDelegate : NSTextContentManagerDelegate
	{
		[Export ("textContentStorage:textParagraphWithRange:")]
		[return: NullAllowed]
		NSTextParagraph GetTextParagraph (NSTextContentStorage textContentStorage, NSRange range);
	}

	interface INSTextContentStorageDelegate  {}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[Protocol]
	interface NSTextStorageObserving
	{
		[Abstract]
		[NullAllowed, Export ("textStorage", ArgumentSemantic.Strong)]
		NSTextStorage TextStorage { get; set; }

		[Abstract]
		[Export ("processEditingForTextStorage:edited:range:changeInLength:invalidatedRange:")]
		void ProcessEditing (NSTextStorage textStorage, NSTextStorageEditActions editMask, NSRange newCharRange, nint delta, NSRange invalidatedCharRange);

		[Abstract]
		[Export ("performEditingTransactionForTextStorage:usingBlock:")]
		void PerformEditingTransaction (NSTextStorage textStorage, Action transaction);
	}

	[TV (15,0), NoWatch, Mac (12,0), iOS (15,0), MacCatalyst (15,0)]
	[BaseType (typeof (NSTextContentManager))]
	interface NSTextContentStorage : NSTextStorageObserving
	{
		[Wrap ("WeakDelegate")]
		[NullAllowed]
		INSTextContentStorageDelegate Delegate { get; set; }

		[NullAllowed, Export ("delegate", ArgumentSemantic.Weak)]
		NSObject WeakDelegate { get; set; }

		[NullAllowed, Export ("attributedString", ArgumentSemantic.Copy)]
		NSAttributedString AttributedString { get; set; }

		[Export ("attributedStringForTextElement:")]
		[return: NullAllowed]
		NSAttributedString GetAttributedString (NSTextElement textElement);

		[Export ("textElementForAttributedString:")]
		[return: NullAllowed]
		NSTextElement GetTextElement (NSAttributedString attributedString);

		[Export ("locationFromLocation:withOffset:")]
		[return: NullAllowed]
		INSTextLocation GetLocation (INSTextLocation location, nint offset);

		[Export ("offsetFromLocation:toLocation:")]
		nint GetOffset (INSTextLocation from, INSTextLocation to);

		[Export ("adjustedRangeFromRange:forEditingTextSelection:")]
		[return: NullAllowed]
		NSTextRange GetAdjustedRange (NSTextRange textRange, bool forEditingTextSelection);
	}

}
