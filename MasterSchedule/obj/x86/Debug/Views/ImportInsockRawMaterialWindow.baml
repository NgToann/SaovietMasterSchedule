   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   8    2MasterSchedule.Views.ImportInsockRawMaterialWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5          � Title$;  5Master Schedule - Import Insock Raw Material Schedule��5   	   $	��480��6G   $	��720��6T    � WindowStartupLocation$ CenterScreen=�5	   !   .��5
      �  � RowDefinitions 5   
   �� 5      $��* �6   �� 5      $
��Auto �6   �� 5      $
��Auto �6   .+�5   
   &   System.Windows.Controls.DataGrid  -   5          Name$ dgInsockRawMaterial��5         AutoGenerateColumns .     
IsReadOnly .    AlternatingRowBackground
 ����� �� AlternationCount$ 2��5   8      HorizontalGridLinesBrush
 �����	   VerticalGridLinesBrush
	 �����
 � 	Resources
 5      *  $System.Windows.Controls.DataGridCell �� 
TargetType �� Triggers P� Property P� Value �� Property �� Value��%�   '        �� 5      "  6    5      P� 5       �@ 
IsSelected . . �5      ��  � ��
 ��iA�6;   �� 5       � ��
 �����6;   5      5      5         Columns 5      0  *System.Windows.Controls.DataGridTextColumn  5      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 6%      Header$ 	Prod. No.��62   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6E   �� *	ProductNo+  5      $
 Auto 6%   $ Insock Supplier��62    6K   �� *InsockSupplierName+  5       $
 Auto 6%   $	 EFD��62    6?   �� *ETD+ �� StringFormat$ 
MM/dd/yyyy��5"      �� 5#   
   $��1��6   $��0,5,0,0q�6"    �� Orientation$ 
Horizontal=�63   $��Right=�6L   .+�5$      �� -   5         ��Name$ 	btnImport��5$      $	��150��6(   $��Right=�67   $��Import��6S    � ToolTip$&  Click to Import to Orders Table.��6d   $
��Bold"�6�   {�.  6�   5%      �� 5&   
   $��2��6   .@�5'      �� $��Processing:��6   � 5(      -   5         �Name$ progressBar��5(      $	��100��6/   $��15��6;    � Minimum$ 0Q�6G    � Value$ 0Q�6S   �� 5)      -   5         ��Name$ 	lblStatus��5)      $�� ��6+   $
��Bold"�63   5*      5+      5,      