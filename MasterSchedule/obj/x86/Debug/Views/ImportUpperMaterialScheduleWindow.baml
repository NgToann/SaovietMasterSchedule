   M S B A M L   `   `   `  ����  MasterSchedule<    6MasterSchedule.Views.ImportUpperMaterialScheduleWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5        $��13(�5   @     � WindowStartupLocation$  CenterScreen=�6N    � Title$6 0Master Schedule - Import Upper Material Schedule��5   	   $	��400��6B   $	��700��6O   .��5      � $��2q�6    � RowDefinitions 5   
   �� 5      $
��auto �6   �� 5      $��* �6   �� 5	      $
��auto �6   .+�5   
   �� $��Center=�6   $��Center=�6/   $��20(�6L   $��Times New Roman)�6Z   $��SemiBold"�6w   $��Upper Material Schedule��6�   $��0,0,0,5q�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgUpperMaterialSchedule��5       $��1��6      AutoGenerateColumns .   �� AlternationCount$ 2��6]      AlternatingRowBackground
 �����   CanUserSortColumns .    CanUserAddRows .  	   SelectionUnit$	 FullRow=�5   .   
   HorizontalGridLinesBrush

 �����   VerticalGridLinesBrush
 �����   RowHeaderWidth$	 NaNQ�6�      
IsReadOnly . $��Times New Roman)�5   )   $��14(�6F      FrozenColumnCount$ 2��6T      	CellStyle 5      �� 5       �� 
TargetType*  $System.Windows.Controls.DataGridCell"  6    �� Triggers 5      P� 5       P� Property �@ 
IsSelected P� Value . . �5      ��  �� Property � �� �� Value
 �   �6;   �� 5       � ��
 ��� �6;   �� 5       � ��$ Semibold"�6;   �� 5       � ��$ 0,0,0,0q�6@   5      5         ColumnHeaderStyle 5      �� 5      =  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  � ��$ Center=�6C   �� 5       � ��$ 15(�61   �� 5       � ��
 �����63   �� 5        � ��
 �����64   �� 5!       � ��$ 0.5,0.5,0.5,0q�68   �� 5"       � ��$ 5q�60   5#         Columns 5%      4  .System.Windows.Controls.DataGridTemplateColumn  5&      ,  &System.Windows.Controls.DataGridColumn   Header$ 	ProductNo��6)      SortMemberPath$ 	ProductNo��6<      CellTemplate 5'      �� 5(      .R�5)      �� $��3q�6C   ��6(   �� *	ProductNo+6C   5*      5,        5-      $ Material Type��6)      MinWidth$	 100Q�6@   $ MaterialTypeName��6O    5.      �� 5/      .R�50      �� $��3q�6J   ��6(   �� *MaterialTypeName+6J   51      53        54      $	 EFD��6)   $	 100Q�66   $	 ETD��6E    55      �� 56      .R�57      �� $��3q�6V   $��Center=�6a   $��Center=�6|   ��6(   �� *ETD+ �� StringFormat$ 
MM/dd/yyyy��6|   58      5:        5;      $ Actual Date��6)   $	 100Q�6>   $ 
ActualDate��6M    5<      �� 5=      .R�5>      �� $��3q�6]   $��Center=�6h   $��Center=�6�   ��6(   �� *
ActualDate+$ 
MM/dd/yyyy��6�   5?      5A      5C      � 5D   
   $��2��6   $��0,2,0,0q�6    � ColumnDefinitions 5E      �� 5F      $��* �6#   �� 5G      $
��auto �6#   .+�5I      �� .@�5J      ��  �� Orientation$ 
Horizontal=�6   .+�5K      �� $��Processing:��6    � 5L      -   5         �Name$ progressBar��5L   "   $	��100��67   $��15��6C     � Minimum$  0Q�6O   ! � Value$! 0Q�6[   $��5,0,5,0q�6e   �� 5M      -   5        " ��Name$" 	lblStatus��5M       $�� ��63   $
��Bold"�6;   5N      5O      �� 5P      -   5        # ��Name$# 	btnImport��5P   %   $��1��6   $��   Import   ��68   $��Right=�6O   6k   5Q      5R      5S      