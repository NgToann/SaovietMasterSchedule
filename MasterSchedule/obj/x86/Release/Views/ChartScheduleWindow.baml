   M S B A M L   `   `   `  ����   MasterSchedule.    (MasterSchedule.Views.ChartScheduleWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     -     � Title$  Chart Schedule��$	��500��$
��Auto�� � WindowState$ 	Maximized=�.��� $��5,5,5,5q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+���  �� Orientation$ 
Horizontal=�.+��� $��Date:��$��Center=�(  "System.Windows.Controls.DatePicker  -     Name$ 
dpDateFrom��$��5,0,0,0q�   SelectedDate$ 3/6/2017X��� $
�� to ��$��Center=�  -   $ dpDateTo��$ 3/6/2017X��� -    ��Name$ 
cboSection��$��5,0,0,0q� �� SelectedIndex$ 0��@��� -   $��S��$��Sewing���� -   $��A��$��Assembly���� -   $��O��$��Outsole���� -    ��Name$ btnViewResult��$��10,0,0,0q�$��Create Chart��$
��Bold"��� -	   	 ��Name$	 lblTitle��$��SCHEDULE CHART��$��20(�$��1��
 �� TextAlignment$
 Center=�&   System.Windows.Controls.DataGrid  -
     Name$ dgAssemblyChart��$��2��   AutoGenerateColumns .     
IsReadOnly .    FrozenColumnCount$ 1��   VerticalGridLinesBrush
 �����   HorizontalGridLinesBrush
 �����   Columns �� $��3��$ 
Horizontal=�$��0,5,0,0q�.+��� $��Legend: ��$
��Bold"��� 
�������$��1q�._��� 
�������$��25���� $��No Schedule | ��$��5,0,0,0q��� 
�������$��1q�._��� 
��� � �$��25���� $��	Normal | ��$��5,0,0,0q��� 
�������$��1q�._��� 
��� ���$��25���� $#��Mat's Arrival > Start Date | ��$��5,0,0,0q��� 
�������$��1q�._��� 
���  ��$��25���� $��Finish Date > ETD��$��5,0,0,0q�