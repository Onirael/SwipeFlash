   M S B A M L   `   `   `  ����E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  5      J FSwipeFlash.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.CoreSwipeFlash.Core 5      $    SwipeFlash.SettingsMenuControl   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      *localclr-namespace:SwipeFlash     5      Gcore6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.Core     5        � 	Resources  5         ContentStyle �� 
TargetType �� Property �� Value �� 
TargetType  BackgroundLightBrush �� CornerRadius J� Effect3  -System.Windows.Media.Effects.DropShadowEffect   Opacity   ShadowDepth	   
BlurRadius
 � RowDefinitions �� Orientation � ColumnDefinitions  
LGCRegular  FontSizeLarge d� 	IsChecked  ForegroundMainBrush J� OpacityMask �� EndPoint � Color � Offset  RoundIconButton  FontAwesome_Close  ForegroundVeryDarkBrush��%U  &	        7  7  7  7  7  7  7  7  7  7  7  �� 5   
   " ��6%   .�5      ��  � �� 5      �� 5      " ��6&   .R�5      �� 8��  $��0q�5   !   $ 10��5   !    5        5   "   $	 0.5Q�63   $ 0Q�6A   $	 10Q�6Q   ._�5      � 
 5   "   �� 5   &   $
��Auto �64   �� 5   &   $
��Auto �64   �� 5   &   $��70 �64   .+�5#   "   �� $ Vertical=�6-   .+�5%   &   � $��0 20 0 0q�6+   $	��210��5&   +   
 5'   *   �� 5)   .   $
��Auto �6<   �� 5+   .   $
��Auto �6<   �� 5-   .   $
��Auto �6<    50   *   �� 52   .   $
��Auto �6?   �� 54   .   $��* �6?   �� 56   .   $
��Auto �6?   .+�5:   *   �� $��0��64   $��0��6A   8�� 8�� $.��(Enable illustrations (internet required)��5=   4   $	��200��5>   4   $
��Wrap=�5?   4   �� 5@   *   $��0��63   $��2��6@   $��Center=�5A   3    5B   3   �� *IllustrationsEnabled+�� 5E   *   $��1��64   $��0��6A   8�� 8�� $��Show instructions��5H   4   $	��200��5I   4   $
��Wrap=�5J   4   $��Center=�5K   4   �� 5L   *   $��1��63   $��2��6@   $��Center=�5M   3   $��0 5 0 0q�5O   3    5N   3   �� *InstructionsVisible+5O   3   �� 5R   *   $��2��64   $��0��6A   8�� 8�� $��Enable sounds��5U   4   $	��200��5V   4   $
��Wrap=�5W   4   $��Center=�5X   4   �� 5Y   *   $��2��63   $��2��6@   $��Center=�5Z   3   $��0 5 0 0q�5\   3    5[   3   �� *SoundsEnabled+5\   3   5]   '   �� 5`   &   $��Manage flashcards...��6-   $��Center=�5a   -   $��0 10 0 0q�5c   -   ��5b   -   �� *OpenManageCardsCommand+5c   -   5d   #   �� 5g   "   $��1��6)   $��50 20q�5h   )   8�� $��1��5j   )    5k   &   �� 5l   *   $	 1 0&�6>   .��5m   .   � � 5n   2   $ 	#00000000��6?   $ 0Q�6Q   � 5o   2   $ 	#ff000000��6?   $ .5Q�6Q   � 5p   2   $ 	#00000000��6?   $ 1Q�6Q   5q   /   5r   +   5t   #   �� 5w   "   $��0 0 0 20q�6)   $��2��5x   )   8�� 8��	 8��
 $��Center=�5}   )   ��5|   )   �� *CloseSettingsCommand+5}   )   5      5�      5�      5�      5�      5�      .��5�      �� #��[  6   5�      