   M S B A M L   `   `   `  ����  CheckUpdate    CheckUpdate.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   -   5          � Title$  Master Schedule Updater��5   	   $	��250��6)   $	��350��66    � WindowStartupLocation$ CenterScreen=�6B    � WindowStyle$ 
ToolWindow=�6g    � 
ResizeMode$ CanMinimize=�6�   .��5      �  � RowDefinitions 5   
   �� 5      �� 5      $
��auto �6   .+�5
   
   �� -   5         ��Name$ 	txtResult��5
      $��Checking...��6'    �� TextAlignment$ Center=�6:   $��Center=�6Q   $��16(�6l   $
��Bold"�6{   �� 5   
   $��1��6   .@�5      �  � ColumnDefinitions 5      �� 5      $
��auto �6'   �� 5      $��* �6'   .+�5      � -   5         �Name$ 	prgStatus��5   E   $��0��6   $��68��6.   $��16��69   6E   �� 5      -   5        	 ��Name$	 	lblStatus��5   G   $��Checking��6   $��26��6+   $��1��67   $��5,0,0,0q�6Z   $��12(�6k   
����  �$��Italic$�6�   5      5      5      5      