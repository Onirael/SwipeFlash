   M S B A M L   `   `   `  ����E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  5      J FSwipeFlash.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.CoreSwipeFlash.Core 5      E AEmoji.Wpf, Version=0.0.0.19, Culture=neutral, PublicKeyToken=null8*clr-namespace:Emoji.Wpf;assembly=Emoji.Wpf	Emoji.Wpf 5	      $    SwipeFlash.AddFlashcardsWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      *localclr-namespace:SwipeFlash     5      Gcore6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.Core     5      <emoji*clr-namespace:Emoji.Wpf;assembly=Emoji.Wpf     5	        � WindowStyle$  
ToolWindow=�5       � WindowStartupLocation$ CenterOwner=�5      $	��400��5      $	��670��6"    � SizeToContent$ Height=�5      $	��400��5       � Title$ Add flashcards��5      .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6   .+�5    
   �� $��0��6   $��Center=�5!      $��Add flashcards��5"      $��20q�5#         FontSizeLarger#��[  5$      �� 5'   
   $��1��6     ForegroundDarkBrush#��[ 5(      $��1q�5)       �� CornerRadius$ 10��5*        BackgroundLightBrush#��[ 5+      $��
10 0 10 20q�5,       �� Padding$ 10q�5-      ._�5.      �� .+�50      �� $��Flashcard family name:��6   $��0 0 0 5q�51        FontSizeLarge#��[ 52      �� 53      #��[ 6   
 � Tag$ Enter a name...��56      ��54      �� *
FamilyName+{�55      �� *IsFamilyDataInputEnabled+56      � 59       � ColumnDefinitions 5:      �� 5;      $
��Auto �6+   �� 5<      $��* �6+   �� 5=      $
��Auto �6+   .+�5A      �� $��0��6!   .+�5C      �� $��Side 1 category��6$   $��0 0 0 5q�5D   $   #��[ 5E   $   �� 5G      $	��4 2q�6#   $	��140��5H   #   $
��Left=�5I   #   	 �� 
IsEditable	 . #��[ 5M   #   ��5J   #   �� *
Categories+
 �� Text
 5L   #   �� *	Category1+{�5N   #   �� *IsFamilyDataInputEnabled+��5O      �� 5P   "   .R�5Q   &   �� ��60   �� 5R   #   5T      �� 5V      $��Category 1 logo:��6$   $��0 0 0 5q�5W   $   #��[ 5X   $   �� 5Z      -   5        $��logo1��5Z   "   $
��Left=�5[   "   $	��100��5\   "   #��[ 5^   "   $ (emoji)��5`   "   ��5]   "   �� *	Side1Logo+{�5_   "   �� *IsLogo1InputEnabled+5`   "   �� 5b      $��Side 1 articles:��6$   $��0 0 0 5q�5c   $   #��[ 5d   $   �� 5f      $
��Left=�6"   $	��140��5h   "   #��[ 5i   "   $ ex: the, a, an��5k   "   ��5g   "   �� *Side1Articles+{�5j   "   �� *IsFamilyDataInputEnabled+5k   "   5l      �� 5n      $��2��6!   .+�5p      �� $��Side 2 category��6$   $��0 0 0 5q�5q   $   #��[ 5r   $   �� 5t      $	��4 2q�6#   $	��140��5u   #   $
��Left=�5v   #    �� DisplayMemberPath$  ��5x   #   	 . #��[ 5z   #   ��5w   #   �� *
Categories+
 5{   #   �� *	Category2+{�5|   #   �� *IsFamilyDataInputEnabled+��5}      �� 5~   "   .R�5   &   �� ��60   �� 5�   #   5�      �� 5�      $��Side 2 logo:��6$   $��0 0 0 5q�5�   $   #��[ 5�   $   �� 5�      $
��Left=�6"   $	��100��5�   "   #��[ 5�   "   $ (emoji)��5�   "   ��5�   "   �� *	Side2Logo+{�5�   "   �� *IsLogo2InputEnabled+5�   "   �� 5�      $��Side 2 articles:��6$   $��0 0 0 5q�5�   $   #��[ 5�   $   �� 5�      $
��Left=�6"   $	��140��5�   "   #��[ 5�   "   $ ex: el, la, los, las��5�   "   ��5�   "   �� *Side2Articles+{�5�   "   �� *IsFamilyDataInputEnabled+5�   "   5�      5�      5�      5�      �� 5�   
   $��2��6   #��[ 5�      $��1q�5�      $ 10��5�      #��[ 5�      $��
10 0 10 20q�5�      ._�5�      �� $
��Auto=�6   $��10q�6>   .��5�      �� .+�5�      �� $��1��6!    �� Orientation$ 
Horizontal=�6.   .+�5�      �� $��From file: ��6$   #��[  5�   $   $��0��5�   $   $��Center=�5�   $   �� 5�      $��Select file...��6!   $��	10 0 10 0q�5�   !   $��25��5�   !   $
��10 0q�5�   !   $��Center=�5�   !     FontSizeRegular#��[ 5�   !   ��5�   !   �� *SelectFileCommand+5�   !   5�      (� 5�      #��[ 6   $
��0 10q�5�      $
��Left=�5�      y�5�      �� *IsFileSelected+ �� 	Converter 6   0SwipeFlash.BooleanToCollapsedVisibilityConverter  .��5�      �� #��[ 5�   (   #��[  5�   (   $��Center=�5�   (   $
��Left=�5�   (   $��CharacterEllipsis=�5�   (   ��5�   $   �� *SelectedFileDisplayName+ �� FallbackValue$ No file selected��5�   (   5�      �� 5�      $��1��6!   y�5�   !   �� *	IsFileTXT+   .+�5�      �� .+�5�      �� $ ��Ignore lines with pattern:��6(   $��0 0 0 5q�5�   (   #��[ 5�   (   �� 5�      #��[ 6&   $ 	ex: #*,*;��5�   &   ��5�   &   �� *IgnorePatternDescription+5�   &   �� 5�      $"��Separators (tab by default):��6(   $��0 0 0 5q�5�   (   #��[ 5�   (   �� 5�      #��[ 6&    �� 
AcceptsTab . $ /,;,+��5�   &   ��5�   &   �� *SeparatorsDescription+5�   &   �� 5�      $��Line pattern:��6(   $��0 0 0 5q�5�   (   #��[ 5�   (   �� 5�       �� TextWrapping$
 Wrap=�6&   ��5�   &   �� *LinePatternDescription+.��6N   $&  ex: [1],[gender],[2]; + commands��5�   /   .��5�      5�      5�      5�      5�      5�      �� 5�   
   $��3��6   $ 
Horizontal=�5�      $��Right=�5�      $��0 0 10 0q�5�      .+�5�      �� $��Cancel��6   $��0 0 10 0q�5�      $��40 10q�5�      $��Center=�5�      #��[ 5�        
LGCRegular#��[ 5�      ��5�      �� *PressCancelCommand+�� 5�      $��3��6   $��Right=�5�      $��OK��5�      #��[ 5      ��5�      �� *PressOKCommand+5      5     5     5     