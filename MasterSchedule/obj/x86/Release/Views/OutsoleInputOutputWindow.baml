   M S B A M L   `   `   `  ����   MasterSchedule3    -MasterSchedule.Views.OutsoleInputOutputWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$,  &Master Schedule - Input Outsole Output��$
��Auto��$	��800�� � SizeToContent$ Height=� � WindowStartupLocation$ CenterScreen=�.���  � RowDefinitions �� $
��Auto ��� $��* ��� $
��Auto �.+�� $��0�� � ColumnDefinitions �� ����  �� ElementName$ dgOutsoleMaterial�� �� Path$ BorderThickness.Left��� ���� $ dgOutsoleMaterial��$ RowHeaderWidth��� $��* �.+��� $��3��
���   �$��1,1,1,0q�._��� $��0,3,0,3q�$
��SIZE�� �� TextAlignment$ Center=�$
��Bold"�&   System.Windows.Controls.DataGrid  -     Name$ dgOutsoleMaterial��$��1��	   RowHeaderWidth$	 10Q�
   AutoGenerateColumns
 .     CanUserAddRows .     FrozenColumnCount$ 1��   SelectionUnit$ CellOrRowHeader=� �� AlternationCount$ 2��   AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   ColumnHeaderStyle ��  �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  .���  �� Property � �� �� Value$
 Bold"���  � ��$ Center=�   Columns 4  .System.Windows.Controls.DataGridTemplateColumn  -   colCompleted��   CellTemplate �� .R���  ��Name$ btnCompleted��-   $��OK���� -   $ btnSave��$��2��$��0,5,0,0q�$	��150��$��Right=�$
��Save��$
��Bold"�{�.  