   M S B A M L   `   `   `  ���� E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  J FSwipeFlash.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.CoreSwipeFlash.Core E AEmoji.Wpf, Version=0.0.0.19, Culture=neutral, PublicKeyToken=null8*clr-namespace:Emoji.Wpf;assembly=Emoji.Wpf	Emoji.Wpf $    SwipeFlash.AddFlashcardsWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:SwipeFlash     Gcore6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.Core     <emoji*clr-namespace:Emoji.Wpf;assembly=Emoji.Wpf       � WindowStyle$  
ToolWindow=� � WindowStartupLocation$ CenterOwner=�$	��400��$	��670�� � SizeToContent$ Height=�$	��400�� � Title$ Add flashcards��.���  � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��$��Center=�$��Add flashcards��$��20q�   FontSizeLarger#��[  �� $��1��  ForegroundDarkBrush#��[ $��1q� �� CornerRadius$ 10��  BackgroundLightBrush#��[ $��
10 0 10 20q� �� Padding$ 10q�._��� .+��� $��Flashcard family name:��$��0 0 0 5q�  FontSizeLarge#��[ �� #��[ 
 � Tag$ Enter a name...������ *
FamilyName+{��� *IsFamilyDataInputEnabled+�  � ColumnDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0��.+��� $��Side 1 category��$��0 0 0 5q�#��[ �� $	��4 2q�$	��140��$
��Left=�	 �� 
IsEditable	 . #��[ ���� *
Categories+
 �� Text
 �� *	Category1+{��� *IsFamilyDataInputEnabled+���� .R��� ���� �� $��Category 1 logo:��$��0 0 0 5q�#��[ �� -   $��logo1��$
��Left=�$	��100��#��[ $ (emoji)������ *	Side1Logo+{��� *IsLogo1InputEnabled+�� $��Side 1 articles:��$��0 0 0 5q�#��[ �� $
��Left=�$	��140��#��[ $ ex: the, a, an������ *Side1Articles+{��� *IsFamilyDataInputEnabled+�� $��2��.+��� $��Side 2 category��$��0 0 0 5q�#��[ �� $	��4 2q�$	��140��$
��Left=� �� DisplayMemberPath$  ��	 . #��[ ���� *
Categories+
 �� *	Category2+{��� *IsFamilyDataInputEnabled+���� .R��� ���� �� $��Side 2 logo:��$��0 0 0 5q�#��[ �� $
��Left=�$	��100��#��[ $ (emoji)������ *	Side2Logo+{��� *IsLogo2InputEnabled+�� $��Side 2 articles:��$��0 0 0 5q�#��[ �� $
��Left=�$	��140��#��[ $ ex: el, la, los, las������ *Side2Articles+{��� *IsFamilyDataInputEnabled+�� $��2��#��[ $��1q�$ 10��#��[ $��
10 0 10 20q�._��� $
��Auto=�$��10q�.���� .+��� $��1�� �� Orientation$ 
Horizontal=�.+��� $��From file: ��#��[  $��0��$��Center=��� $��Select file...��$��	10 0 10 0q�$��25��$
��10 0q�$��Center=�  FontSizeRegular#��[ ���� *SelectFileCommand+(� #��[ $
��0 10q�$
��Left=�y��� *IsFileSelected+ �� 	Converter 6   0SwipeFlash.BooleanToCollapsedVisibilityConverter  .���� #��[ #��[  $��Center=�$
��Left=�$��CharacterEllipsis=����� *SelectedFileDisplayName+ �� FallbackValue$ No file selected���� $��1��y��� *	IsFileTXT+   .+��� .+��� $ ��Ignore lines with pattern:��$��0 0 0 5q�#��[ �� #��[ $ 	ex: #*,*;������ *IgnorePatternDescription+�� $"��Separators (tab by default):��$��0 0 0 5q�#��[ �� #��[  �� 
AcceptsTab . $ /,;,+������ *SeparatorsDescription+�� $��Line pattern:��$��0 0 0 5q�#��[ ��  �� TextWrapping$
 Wrap=����� *LinePatternDescription+.��$&  ex: [1],[gender],[2]; + commands��.���� $��3��$ 
Horizontal=�$��Right=�$��0 0 10 0q�.+��� $��Cancel��$��0 0 10 0q�$��40 10q�$��Center=�#��[   
LGCRegular#��[ ���� *PressCancelCommand+�� $��3��$��Right=�$��OK��#��[ ���� *PressOKCommand+