   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   -    'MasterSchedule.Views.PrintSizeRunWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5        $��13(�5   P     � WindowStartupLocation$  CenterScreen=�5   :    � Title$&  Master Schedule - Print Size Run��5   	   $	��450��62   $	��800��6?   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      $��* �6   �� 5      $
��Auto �6    � ColumnDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   .+�5   
   ��    GroupBoxTemplate#���   6   ��5      �� 5        GroupBackground#���  6     BorderStyle#���  6T   ._�5      �� $��Input ProductNoList��6    
���   � �� Padding$ 	10,0,10,0q�6N   5      .��5      �  5      �� 5      $
��auto �6$    5      �� 5      $��* �6'   �� 5      $��10 �6'   �� 5       $
��auto �6'   .+�5"      �� -   5         ��Name$ txtProductNoList��5"      $	��150��64   $	��300��6C   $��60��6R   $	��360��6a    �� TextWrapping$
 Wrap=�6q   $��3q�6�    � ToolTip$ Separate PO by ;��6�   �� 5#      $��2��6   .+�5$      �� -   5         ��Name$ 
btnPreview��5$      $��80��61   $��60��6?   $��60��6N   $��  _Preview  ��6]     ButtonTemplate#���  6t   	 �� 	IsDefault	 . 6�   5%      5'      5(      &   System.Windows.Controls.DataGrid  5)   
   -   5        
  Name$
 	dgSizeRun��5)   E   $��1��6   $��2��6    $��0,5,0,0q�64      AutoGenerateColumns .   �� AlternationCount$ 2��6t      AlternatingRowBackground
 �����   CanUserSortColumns .    CanUserAddRows .     SelectionUnit$
 Cell=�5*   .      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   RowHeaderWidth$	 NaNQ�6�      
IsReadOnly . $��Times New Roman)�5+   )   $��14(�6F      	CellStyle 5,      �� 5-       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5.      P� 5/       P� Property �@ 
IsSelected P� Value . . �50      ��  �� Property � �� �� Value
 �   �6;   �� 51       � ��
 ��� �6;   �� 52       � ��$ Semibold"�6;   �� 53       � ��$ 0,0,0,0q�6@   54      56         ColumnHeaderStyle 58      �� 59      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5:      ��  � ��$ Center=�6C   �� 5;       � ��$ 15(�61   �� 5<       � ��
 �����63   �� 5=       � ��
 �����64   �� 5>       � ��$ 0.5,0.5,0.5,0q�68   �� 5?       � ��$ 8,5,5,8q�60   5@      5B      � 5C   
   $��2��6   $��2��6   $��0,2,0,0q�60    5D      �� 5E      �� 5F      $
��auto �6#   .+�5H      �� -   5        $ btnPrint��5H   %   $��1��6   $��80��67   $��
  _Print  ��6E   #���  6Z   	 . $��27��6�   $	��Top=�6�   6�   5I      5J      5K      