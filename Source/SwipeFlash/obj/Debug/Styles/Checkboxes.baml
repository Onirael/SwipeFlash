   M S B A M L   `   `   `  ����E  ASwipeFlash, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null'clr-namespace:SwipeFlash
SwipeFlash  5      O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem 5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089  �� 
TargetType   	BaseStyle �� BasedOn �� Property �� Value  BackgroundLightBrush  SwipeFlashDarkVioletBrush  ForegroundDarkBrush  
LGCRegular  FontSizeLarge �� 
TargetType J� RenderTransformOrigin ��Name J� SnapsToDevicePixels �� CornerRadius  FontAwesomeSolid  FontAwesome_Check  FontSizeLarger	 �� 	Converter-    'SwipeFlash.BooleanToVisibilityConverter
 �� Triggers :� RoutedEvent �� 
TargetName �� TargetProperty	 W� To g� Duration P� Property P� Value 	 ForegroundMainBrush�� 5      D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    :x,http://schemas.microsoft.com/winfx/2006/xaml     5      "localclr-namespace:SwipeFlash  5      1System&clr-namespace:System;assembly=mscorlib  5      %�  '��       7   7  7  7  7  7  7  7  7  7  7 	 �� 5      "  ��6   8   .�5   
   ��  � ��$ 30��6#   �� 5
   
    � ��8  6'   �� 5   
    � ��8  6'   �� 5   
    � ��$ 1q�6,   �� 5   
    � ��8  6(   �� 5   
    �@d�	IsChecked$ FalseY�6&   �� 5   
    � ��8  6'   �� 5   
    � ��8  6%   �� 5   
    � ��$ 50 10q�6$   �� 5   
    � ��$
 0 10q�6#   �� 5   
    � �� 5      �� 5      " ��6"   .R�5      (� .��5      � $ .5 .5&�6   z�5      V� 5   "   .~�5   &   �� checkboxScaleTransform��65   5   #   .+�5"      �� $ border��6%   #��z��5#   !   #��z��5$   !   #��z��5%   !    . $ 5��5'   !   $��Center=�5(   !   $��Center=�5)   !   $��20��5*   !   $��20��5+   !   �� 5-      #��z��6(   8�� 8�� 8�� $��6 0 0 5q�51   (   y�52   (   �� *	IsChecked+	    53      54      
 55      :� 57      $ PreviewMouseDown��6'   .[�58      �� .��59   "   �� .��5:   &   W� $ checkboxScaleTransform��66   $ (ScaleTransform.ScaleX)�6e   $ .9X�6�   $ 0:0:0.05D�6�   W� 5;   &   $ checkboxScaleTransform��66   $ (ScaleTransform.ScaleY)�6e   $ .9X�6�   $ 0:0:0.05D�6�   5<   #   5=      5>      :� 5@      $ PreviewMouseUp��6'   .[�5A      �� .��5B   "   �� .��5C   &   W� $ checkboxScaleTransform��66   $ (ScaleTransform.ScaleX)�6e   $ 1X�6�   $ 0:0:0.05D�6�   W� 5D   &   $ checkboxScaleTransform��66   $ (ScaleTransform.ScaleY)�6e   $ 1X�6�   $ 0:0:0.05D�6�   5E   #   5F      5G      P� 5I       � {� .  . �5J      �� 8 	  � ��6;   �� 5K      8 
  � ��6;   5L      5O      5Q      5R      5T      