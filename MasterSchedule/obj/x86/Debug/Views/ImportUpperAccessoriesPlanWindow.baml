   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   ;    5MasterSchedule.Views.ImportUpperAccessoriesPlanWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5        $��13(�5   :     � WindowStartupLocation$  CenterScreen=�5       � Title$5 /Master Schedule - Import Upper Accessories Plan��5   	   $	��450��6A   $	��800��6N   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      �� 5      $
��Auto �6   .+�5   
   �  � ColumnDefinitions 5      �� 5      $
��Auto �6#   �� 5      �� 5      $
��Auto �6#   .+�5      �  5      �� 5      $��* �6$   �� 5      $
��Auto �6$   .+�5      �� -   5         ��Name$ btnOpenExcel��5      $��1��6/   $��  Open Excel File  ��6<   6Z   5      �� 5      -   5        $ btnSupplierInfo��5   %   $��2��6      ButtonNoneTemplate#���   6>    � Cursor$
 Hand��6z   ��5      � 5       5      �� 5       $
��auto �6/   �� 5!      .+�5#      �� $��16��6    $��16��6+   $��/Images/user_config.png��67   �� 5$      $��1��6$   $��3,0,0,0q�64   $��Supplier Info��6E   $��Center=�6Z   5%      5'      5(      &   System.Windows.Controls.DataGrid  5)   
   -   5          Name$ dgMain��5)      $��1��6#   $��0,5,0,0q�60      AutoGenerateColumns .     
IsReadOnly . 	   AlternatingRowBackground
	 �����
 �� AlternationCount$
 2��5*   8      RowHeaderWidth$	 NaNQ�6l      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 ����� � 	Resources 5,      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%�   '        �� 5-      "  6    5.      P� 5/       �@ 
IsSelected . . �50      ��  � ��
 ��iA�6;   �� 51       � ��
 �����6;   52      54      55         Columns 56      0  *System.Windows.Controls.DataGridTextColumn  57      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6%      Header$ Accessories Name��62   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6L   �� *ProvideAccessories+  58      $
 Auto 6%   $ Supplier��62    6D   �� *Name+  59      $
 Auto 6%   $ 	ProductNo��62    6E   �� *	ProductNo+  5:      $
 Auto 6%   $	 ETD��62    6?   �� *ETD+ �� StringFormat$ 
MM/dd/yyyy��5<      � 5=   
   $��2��6   $��0,2,0,0q�6    5>      �� 5?      �� 5@      $
��Auto �6#   .+�5B      ��  �� Orientation$ 
Horizontal=�6   $��Bottom=�62   .+�5C      � -   5         �Name$ 	prgStatus��5C      $	��100��61   $��18��6=   �� 5D      -   5         ��Name$ 	txtStatus��5D      $��10,0,0,0q�6/   $��Center=�6A   
����  �$��Italic$�6n   5E      �� 5F      -   5        $ 	btnImport��5F        ButtonTemplate#���  6(   $��
  Import  ��6`   $��1��6u   6�   5G      5H      5I      