   M S B A M L   `   `   `  ����  MasterSchedule5    /MasterSchedule.Views.RawMaterialSearchBoxWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Master Schedule - Search��5   	   $	��180��6*   $	��420��67    � WindowStartupLocation$ CenterScreen=�6C    � 
ResizeMode$ NoResize=�6h    � Topmost . .��5      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5	      $
��Auto �6   .+�5   
   �� .@�5      �� $
��Find��6   .��5      � $��5,5,5,5q�6    � ColumnDefinitions 5      �� 5      $
��Auto �6+   �� 5      $��* �6+    5      �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   .+�5      �� $��0��6    $��
Find What:��60   $��Bottom=�6B   �� 5      -   5         ��Name$ txtFindWhat��5      $��1��63   $��5,0,0,0q�6C   
�������6T   �� 5      -   5         ��Name$ 
cboIsMatch��5      $��1��63   $��1��6C   $��5,5,0,0q�6P   $��
Match Case��6a    d� 	IsChecked$
 TrueY�6v   6�   �� 5      -   5        	 ��Name$	 
spShowHide��5   !   
 �� Orientation$
 
Horizontal=�65   $��1��6N   $��2��6^   $��5,5,0,0q�6k   $y�	Collapsed=�6|   .+�5      	� -   5         	�Name$ rbShow��5   &   $
��Show��66   $
 TrueY�6E   	� 5      $
��Hide��6&   $��25,0,0,0q�65   5      5      5      5       �� 5!   
   $��1��6   $��0,5,0,0q�6"   $
 
Horizontal=�63   $��Right=�6L   .+�5"      �� -   5         ��Name$ 
btnFindAll��5"      $��Find All��6)   $��75��6<    �� 	IsDefault . 6`   �� 5#      -   5        $ btnClose��5#      $��10,0,0,0q�6'   $��Close��69   $��75��6I   6T   5$      5%      5&      