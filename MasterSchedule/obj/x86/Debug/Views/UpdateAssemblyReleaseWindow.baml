   M S B A M L   `   `   `  ����  MasterSchedule6    0MasterSchedule.Views.UpdateAssemblyReleaseWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$+  %Master Schedule - Release to Assembly��5   	   $	��480��67   $	��900��6D    � WindowStartupLocation$ CenterScreen=�5   
   .��5      �  � RowDefinitions 5   
   �� 5      $
��Auto �6   �� 5	      $��* �6   �� 5
      $
��Auto �6   .+�5   
   �� $��0,10,0,0q�6   $��RELEASE(ASSEMBLY)��6&    �� TextAlignment$ Center=�6?   $
��Bold"�6V   $��15(�6h   �� 5   
    �� Orientation$ 
Horizontal=�6   $��Right=�6.   $��Bottom=�6J   .+�5      �� $��
Report ID:��6   �� 5      -   5         ��Name$ lblReportId��5      $��5,0,5,0q�6-   $�� ��6>   $
��Bold"�6F   5      �� 5   
   -   5         ��Name$ svMain��5      $��1��6'   .��5      �� -   5         ��Name$ spMain��5      $ Vertical=�6)   5      5      � 5   
   $��2��6   $��0,5,0,0q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   �� 5      $��* �6#   �� 5      $
��Auto �6#   �� 5      $
��Auto �6#   .+�5      �� -   5        	 ��Name$	 
btnAddMore��5      $��0��6)   $	��150��69   $��Other Product No.��6H   {�.  6}   �� 5      -   5        $	 	btnExport��5      $��4��6(   $��10,0,0,0q�68   $	��150��6J   $��Report��6Y   {�.  6�   �� 5       -   5        $	 
btnRelease��5       $��3��6)   $	��150��69   $��Release��6H   $
��Bold"�6Z   {�.  6�   5!      � 5"   
   -   5        
 �Name$
 popupAddMore��5"       � AllowsTransparency . $��Slide=�6@    � 	StaysOpen .  .��5#      �� 
�������$��1,1,1,1q�6(   
�������._�5$      � $��5,5,5,5q�6    5%      �� 5&      $
��Auto �6+   �� 5'      $
��Auto �6+    5)      �� 5*      $
��Auto �6(   �� 5+      $
��Auto �6(   .+�5-      �� $��Product No.:��6    $��Bottom=�64   �� 5.      -	   5         ��Name$ txtProductNo��5.      $��1��64   $��5,0,0,0q�6D   $�� ��6U   $	��125��6]   �� 5/      -
   5        $	 btnAddMoreOk��5/      $��1��63   $��1��6@   $��5,5,0,0q�6P   $��OK��6a   $
��Bold"�6n   $��50��6�   $��Right=�6�    �� 	IsDefault . 6�   50      51      52      53      54      