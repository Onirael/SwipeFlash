   M S B A M L   `   `   `  ���� E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  J FSwipeFlash.Core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullJ6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.CoreSwipeFlash.Core     SwipeFlash.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     *localclr-namespace:SwipeFlash     Gcore6clr-namespace:SwipeFlash.Core;assembly=SwipeFlash.Core     -      SwipeFlash.BaseWindow`1   Name$
  Page�� � WindowStartupLocation$ CenterScreen=� � Title$ Swipe Flash��$	��450��$	��800��.��� .+���    LightBlueBrush#��[   J� Opacity$	 0.7Q� J� OpacityMask �� .��� �  � Offset$ 0Q� � Color$ 	#00ffffff��� $	 0.5Q�$ 	#ffffffff��� $ 1Q�$ 	#00ffffff���� -   $��gradient��  BackgroundLightBrush#��[  
�  
� GradientOrigin$
 .5 1&� 
� Center$
 .5 1&�	 �� Opacity$	 .7Q�
 
� RadiusX$
 1Q� 
� RadiusY$ 2Q���� $ 0Q�$ 	#ffffffff��� $ .4Q�$ 	#80ffffff��� $ 1Q�$ 	#00ffffff���� �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateGradientBrushXRadiusProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value �� *IsCardSwipedRight+��
� $ 1.1 .5&�$ 1.1 .5&�$	 1Q�$	
 .35Q�$ 1Q�.��� $ 0Q�$ 	#ff00ff00��� $ .3Q�$ 	#8000ff00��� $ 1Q�$ 	#0000ff00����  �� *IsCardSwipedLeft+��
� $ -0.1 .5&�$ -0.1 .5&�$	 1Q�$
 .3Q�$ 1Q�.��� $ 0Q�$ 	#ffff0000��� $ .3Q�$ 	#80ff0000��� $ 1Q�$ 	#00ff0000��� �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateContentFadeInProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value �� *'%ApplicationViewModel.IsContentVisible+!   SwipeFlash.ViewModelLocator   Instance �� Source# Z .+�   SwipeFlash.FlashcardHost  $��75 50 75 25q��� $��30��
���  ��$	��Top=��   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateSlideInFromTopProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value �� *31ApplicationViewModel.IsNetworkErrorMessageVisible+# Z ._��� $9��3Could not communicate with the illustrations server��$��Center=�#��[ �� $9��3pack://application:,,,/Resources/SwipeFlashIcon.png��$��50q� J� IsHitTestVisible .  �   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateScaleAndFadeInProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value �� *&$ApplicationViewModel.IsContentLoaded+# Z  �� 	Converter '   !SwipeFlash.InvertBooleanConverter  � $
��Left=�$��Bottom=�$��20q�y��� *InstructionsVisible+ 6	   0SwipeFlash.BooleanToCollapsedVisibilityConverter	   � ColumnDefinitions �� $
��Auto ��� $
��Auto � � RowDefinitions �� $
��Auto ��� $
��Auto �.+���   FontAwesome_ArrowKeys#��[ #��[   FontAwesomeSolid#��[ $
��10 0q�$��Bottom=�$��Center=�  FontSizeLarge#��[ $��0��$��0���� $��Swipe��#��[ #��[   LGCBold#��[ $��0��$��1���� $��SPACE��#��[ #��[ $
��10 0q�$��Center=�  FontSizeRegular#��[ $��1��$��0���� $��	Flip card��#��[ #��[ #��[ $��1��$��1��$
   SwipeFlash.SettingsMenuControl
  $	��Top=�$��	0 -10 0 0q�$	��500���   �SwipeFlash.BaseAttachedProperty`2[[SwipeFlash.AnimateSettingsSlideInProperty, SwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null],[System.Boolean, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Value �� *,*ApplicationViewModel.IsSettingsMenuVisible+# Z 