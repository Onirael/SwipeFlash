   M S B A M L   `   `   `  ����E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  5      J FSwipeFlash.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.CoreSwipeFlash.Core 5          SwipeFlash.MainWindow   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      *localclr-namespace:SwipeFlash     5      Gcore6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.Core     5      -   5           SwipeFlash.BaseWindow`1   Name$
  Page��5       � WindowStartupLocation$ CenterScreen=�5
       � Title$ Swipe Flash��5      $	��450��6'   $	��800��64   .��5      � .+�5   
   ��    LightBlueBrush#��[  6    J� Opacity$	 0.7Q�6>    J� OpacityMask 5      �� 5      .��5      � � 5       � Offset$ 0Q�6'    � Color$ 	#00ffffff��62   � 5      $	 0.5Q�6'   $ 	#ffffffff��64   � 5      $ 1Q�6'   $ 	#00ffffff��62   5      5      5      �� 5   
   -   5        $��gradient��5        BackgroundLightBrush#��[ 6!    5!      
� 5"       
� GradientOrigin$
 .5 1&�6&    
� Center$
 .5 1&�6<   	 �� Opacity$	 .7Q�6J   
 
� RadiusX$
 1Q�6W    
� RadiusY$ 2Q�6c   ��5$      � $ 0Q�6'   $ 	#ffffffff��62   � 5%      $ .4Q�6'   $ 	#80ffffff��63   � 5&      $ 1Q�6'   $ 	#00ffffff��62   5(      5*      �� 5-   
   �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateGradientBrushXRadiusProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 6   �� *IsCardSwipedRight+��5.      
� 5/      $ 1.1 .5&�6&   $ 1.1 .5&�6>   $	 1Q�6N   $	
 .35Q�6Z   $ 1Q�6h   .��50      � $ 0Q�6#   $ 	#ff00ff00��6.   � 51      $ .3Q�6#   $ 	#8000ff00��6/   � 52      $ 1Q�6#   $ 	#0000ff00��6.   53      55      �� 58   
    6   �� *IsCardSwipedLeft+��59      
� 5:      $ -0.1 .5&�6&   $ -0.1 .5&�6?   $	 1Q�6P   $
 .3Q�6\   $ 1Q�6i   .��5;      � $ 0Q�6#   $ 	#ffff0000��6.   � 5<      $ .3Q�6#   $ 	#80ff0000��6/   � 5=      $ 1Q�6#   $ 	#00ff0000��6.   5>      5@      � 5D   
   �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateContentFadeInProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 6   �� *'%ApplicationViewModel.IsContentVisible+!   SwipeFlash.ViewModelLocator   Instance �� Source# Z .+�5G         SwipeFlash.FlashcardHost  $��75 50 75 25q�5H   "   �� 5L      $��30��6   
���  ��$	��Top=�5N      �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateSlideInFromTopProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 5O      �� *31ApplicationViewModel.IsNetworkErrorMessageVisible+# Z ._�5Q      �� $9��3Could not communicate with the illustrations server��6   $��Center=�5R      #��[ 5S      5T      5X      �� 5\   
   $9��3pack://application:,,,/Resources/SwipeFlashIcon.png��6   $��50q�5]       J� IsHitTestVisible .  �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateScaleAndFadeInProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 5^      �� *&$ApplicationViewModel.IsContentLoaded+# Z  �� 	Converter '   !SwipeFlash.InvertBooleanConverter  5a      � 5f   
   $
��Left=�6   $��Bottom=�5g      $��20q�5h      y�5i      �� *InstructionsVisible+ 6	   0SwipeFlash.BooleanToCollapsedVisibilityConverter	   � ColumnDefinitions 5k      �� 5l      $
��Auto �6#   �� 5m      $
��Auto �6#    � RowDefinitions 5p      �� 5q      $
��Auto �6    �� 5r      $
��Auto �6    .+�5v      ��   FontAwesome_ArrowKeys#��[ 6   #��[ 5w        FontAwesomeSolid#��[ 5x      $
��10 0q�5y      $��Bottom=�5z      $��Center=�5{        FontSizeLarge#��[ 5|      $��0��5}      $��0��6%   �� 5~      $��Swipe��6   #��[ 5      #��[ 5�        LGCBold#��[ 5�      $��0��5�      $��1��6%   �� 5�      $��SPACE��6   #��[ 5�      #��[ 5�      $
��10 0q�5�      $��Center=�5�        FontSizeRegular#��[ 5�      $��1��5�      $��0��6%   �� 5�      $��	Flip card��6   #��[ 5�      #��[ 5�      #��[ 5�      $��1��5�      $��1��6%   5�      $
   SwipeFlash.SettingsMenuControl
  5�   
   $	��Top=�6$   $��	0 -10 0 0q�5�   $   $	��500��5�   $   �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateSettingsSlideInProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value 5�   $   �� *,*ApplicationViewModel.IsSettingsMenuVisible+# Z 5�      5�      