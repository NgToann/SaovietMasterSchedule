   M S B A M L   `   `   `  ����  MasterSchedule-    'MasterSchedule.Views.UpdateOrdersWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35U QWPFToolkit, Version=3.5.40128.1, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35H 9http://schemas.microsoft.com/winfx/2006/xaml/presentation      5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$%  Master Schedule - Update Orders��5   	   $
��Auto��61    � SizeToContent$ Height=�6?   $
��1000��6V    � WindowStartupLocation$ CenterScreen=�6f   .��5      �� $
��Auto=�6   .��5   
   � $��3q�6    � RowDefinitions 5      �� 5      $
��Auto �6    �� 5	      $
��Auto �6    .+�5      ��    GroupBoxTemplate#���   6   ��5      �� 5       �� Padding$ 4q�6     Background1#���  6)   ._�5      �� $��Update Order, SizeRun��6$   $��14(�6A   $��Italic$�6O   $
��Bold"�6b   5      .��5      �  5      �� 5      $
��Auto �6(   �� 5      �� 5      $
��Auto �6(   �� 5      $
��Auto �6(   �� 5      �� 5      $
��Auto �6(   .+�5      ��  �� Orientation$ 
Horizontal=�6!   .+�5      �� $��	Prod No.:��6$   $��Bottom=�65   $
��Bold"�6P   �� 5      -   5         ��Name$ txtProductNo��5   "   $��5,0,0,0q�68   $	��100��6I   
���   �$��1,1,1,1q�6l   6�   �� 5      -   5         ��Name$ 	btnSearch��5   !   $��10,0,0,0q�64   $��75��6F   $��Search��6T    �� 	IsDefault . $
��Bold"�6v   6�   �� 5      -   5        $ 	btnDelete��5   !   $��10,0,0,0q�64   $��75��6F   $��Delete��6T   $
��Bold"�6e   6w   �� 5       -   5        $ btnUpdateCSDPatternNo��5    !   $��10,0,0,0q�6@   $��  Specify Update  ��6R   $
��Bold"�6o   6�   5!      &   System.Windows.Controls.DataGrid  5"      -   5        	  Name$	 dgOrder��5"   ,   $��1��6   $��0,2,0,0q�6=   
   CanUserAddRows
 .     SelectionUnit$ CellOrRowHeader=�5$         AutoGenerateColumns .   �� AlternationCount$ 2��5%         AlternatingRowBackground
 �����   HorizontalGridLinesBrush
 �����   VerticalGridLinesBrush
 �����   Columns 5(      0  *System.Windows.Controls.DataGridTextColumn  5)      ,  &System.Windows.Controls.DataGridColumn   Width5  /System.Windows.Controls.DataGridLengthConverter$
 Auto 61      Header$ 	GTNPO No.��6>   1  +System.Windows.Controls.DataGridBoundColumn   Binding 6Q   �� *	GTNPONo+  5*      $
 Auto 61   $ UCustomerCode��6>    6U   �� *UCustomerCode+  5+      $
 Auto 61   $	 EFD��6>    6K   �� *ETD+ �� StringFormat$ 
yyyy-MM-dd��  5,      $
 Auto 61   $ ShipDate��6>    6P   �� *
ShipDate+$ 
yyyy-MM-dd��  5-      $
 Auto 61   $ Article No.��6>    6S   �� *	ArticleNo+  5.      $
 Auto 61   $ 	Shoe Name��6>    6Q   �� *
ShoeName+  5/      $
 Auto 61   $	 QTY��6>    6K   �� *
Quantity+  50      $
 Auto 61   $ 
Pattern No��6>    6R   �� *	PatternNo+  51      $
 Auto 61   $ Midsole��6>    6O   �� *MidsoleCode+  52      $
 Auto 61   $ Outsole Code��6>    6T   �� *OutsoleCode+  53      $
 Auto 61   $
 Last��6>    6L   �� *
LastCode+  54      $
 Auto 61   $ Country��6>    6O   �� *	Country+  55      $
 Auto 61   $ TypeOfShoes��6>    6S   �� *TypeOfShoes+4  .System.Windows.Controls.DataGridCheckBoxColumn  56      $
 Auto 65   $ 	Is Enable��6B    6V   �� *
IsEnable+58      �� 59      -   5        $ btnUpdateOrder��59      $��2��65   $��0,2,0,0q�6B   $	��125��6S   $��Update Order��6b   $��Right=�6y   $
��Bold"�6�   {�.  6�   �� 5:      $��Size Run��6    $��3��60   $
��Bold"�6=     5;      -   5        $	 	dgSizeRun��5;      $��4��62   $��0,2,0,0q�6?   
 .  $ CellOrRowHeader=�6g    .     ColumnHeaderStyle 5<      �� 5=       �� 
TargetType=  7System.Windows.Controls.Primitives.DataGridColumnHeader"  6$   .�5>   "   ��  �� Property � �� �� Value$
 Bold"�6?   �� 5?   "    � ��$ Center=�6O   5@      5B      �� 5C      -   5        $ btnUpdateSizeRun��5C      $��5��67   $��0,2,0,0q�6D   $	��125��6U   $��Update Size Run��6d   $��Right=�6~   $
��Bold"�6�   {�.  6�   5D      5E      �� 5F      $��2��6   $��0,5,0,0q�6$   #���   65   ��5G      �� 5H      $ 4q�6   #���  6)   ._�5I      �� $'��!Update Outsole Size, Midsole Size��6$   $��14(�6M   $��Italic$�6[   $
��Bold"�6n   5J      .��5L      �  5M      �� 5N      $
��auto �6(   �� 5O      $
��auto �6(   �� 5P      $
��auto �6(   �� 5Q      $
��auto �6(   �� 5R      $
��auto �6(   �� 5S      $
��auto �6(   �� 5T      $
��auto �6(   .+�5V      �� $ 
Horizontal=�6!   .+�5W      �� $��Outsole Code: ��6$   $��Bottom=�6:   $
��Bold"�6U   �� 5X      -	   5        $ txtOutsoleCode��5X   "   $	��100��6:   $��5,0,0,0q�6I   
���   �$��1,1,1,1q�6n   6�   �� 5Y      -
   5        $ btnSearchOutsoleCode��5Y   !   $��_Search��6?   $
��Bold"�6Q   $��5,0,0,0q�6d   $��60��6u   6�   �� 5Z      -   5        $0 *btnCheckOutsoleCodeNotYetUpdateSizeOutsole��5Z   !   $��_Check��6U   $
��Bold"�6f   $��5,0,0,0q�6y   $��60��6�   6�   5[        5\      -   5        $	 	dgSizeMap��5\   =   $��0,2,0,0q�6   $��1��60   
 .  $ CellOrRowHeader=�6g    .   5]      �� 5^      "  6$   .�5_   "   ��  � ��$
 Bold"�6?   �� 5`   "    � ��$ Center=�6O   5a      5c      �� 5d      -   5        $ btnUpdateSizeMap��5d      $��2��67   $��0,5,0,0q�6D   $	��125��6U   $��_Update Size��6d   $��Right=�6{   $
��Bold"�6�   {�.  6�   � 5f      -   5         �Name$  prgStatusUpdateOutsoleCode��5f   K   $��3��6"   $��0,5,0,5q�6/   $��8��6@    � Value$ 0Q�6o   
������ 
������ 6�   �� 5h      $ 
Horizontal=�6!   $��4��6:   .+�5i      �� $&�� ArticleNo (exclude color code): ��6$   $��Bottom=�6L   $
��Bold"�6g   �� 5j      -   5        $ txtArticleNo��5j   "   $	��100��68   $��5,0,0,0q�6G   
���   �$��1,1,1,1q�6l   6�   �� 5k      -   5        $ btnSearchArticle��5k   !   $��_Search��6;   $
��Bold"�6M   $��5,0,0,0q�6`   $��60��6q   6   5l        5m      -   5        $	 dgSizeMapByArticle��5m   =   $��0,2,0,0q�6   $��5��60   
 .  $ CellOrRowHeader=�6p    .   5n      �� 5o      "  6$   .�5p   "   ��  � ��$
 Bold"�6?   �� 5q   "    � ��$ Center=�6O   5r      5t      �� 5u      -   5        $ btnUpdateSizeMapByArticle��5u      $��6��6@   $��0,5,0,0q�6M   $	��125��6^   $��_Update Size��6m   $��Right=�6�   $
��Bold"�6�   {�.  6�   5v      5w      5x      5y      5z      