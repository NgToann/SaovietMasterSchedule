   M S B A M L   `   `   `  ����I  EMasterSchedule, Version=1.2.8.9, Culture=neutral, PublicKeyToken=null;"clr-namespace:MasterSchedule.ViewsMasterSchedule.Views  5   	   =    7MasterSchedule.Views.UpperAccessoriesReportDetailWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   4local"clr-namespace:MasterSchedule.Views     5   	   -   5          � WindowState$  	Maximized=�5   :    � WindowStartupLocation$ CenterScreen=�5       � Title$7 1Master Schedule - Upper Accessories Report Detail��5   	   $	��450��6C   $	��800��6P   .��5	      � $��2q�6    � RowDefinitions 5
   
   �� 5      $
��Auto �6   �� 5      .+�5   
   �� -   5         ��Name$ tblTitle��5      $0��*UPPER ACCESSORIES REPORT DETAIL - SUPPLIER��6&   $��Center=�6X   $��17(�6u   $��SemiBold"�6�   &   System.Windows.Controls.DataGrid  5   
   -   5          Name$ dgReportBySupplier��5       $��1��6      AutoGenerateColumns .     
IsReadOnly .    AlternatingRowBackground
 �����	 �� AlternationCount$	 2��5   :   
   RowHeaderWidth$	
 NaNQ�6z      HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ClipboardCopyMode$ IncludeHeader=�5   ]      ColumnHeaderStyle 5      �� 5       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6   .�5      ��  �� Property � �� �� Value$ Center=�6C   �� 5       � ��$ 14(�61   �� 5       � ��
 �����63   �� 5       � ��
 �   �63   �� 5       � ��
 �����64   �� 5       � ��$ 0.5,0.5,0.5,0q�68   �� 5       � ��$ 4q�60   5         	CellStyle 5      �� 5      *  $System.Windows.Controls.DataGridCell"  6   .�5      ��  � ��$ Center=�6C   �� 5        � ��$ Center=�6A   �� 5!       � ��$ 2q�60    �� Triggers 5"      P� 5#       P� Property �@ 
IsSelected P� Value . . �5$      ��  � ��
 �����6;   �� 5%       � ��
 ��iA�6;   �� 5&       � ��$ Semibold"�6;   �� 5'       � ��$ 0,0,0,0q�6@   5(      5*      5,      5-      5.      