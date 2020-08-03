# AdmanDev.Rating


Cette librairie contient un contrôle WPF permettant à l'utilisateur d'évaluer des choses en remplissant des symboles.
Ce contrôle est entièrement personnalisable: degré de précision, lecture seule, couleur, nombre et taille des symboles...

## Démarrage rapide
- Créez un nouveau projet WPF
- Référencez le dll **AdmanDev.Rating**
- Ajoutez la référence XAML
<code>xmlns:rating="clr-namespace:Admandev.Rating;assembly=Admandev.Rating"</code>
- Ajoutez le contrôle
<code><rating:Rating Name="rating"/> </code>


## Degré de précision
<table>
	<thead>
		<tr>
			<th>Standard</th>
			<th>Half</th>
			<th>Exact</th>
		</tr>
	</thead>
	<tbody>
		<tr>
			<td><img src="https://i.ibb.co/fkbHWjF/Rating.png" alt="standard mode"/></td>
	  <td><img src="https://i.ibb.co/d7ynb6J/Rating-Half-mode.png" alt="half mode"/></td>
	  <td><img src="https://i.ibb.co/Hxgxvjd/Rating-Exact-mode.png" alt="exact mode"/></td>
		</tr>
		<tr>
		<td>Le mode standard permet de sélectionner les symboles entièrement.</td>
		<td>Le mode half permet de sélectionner la moitié d'un symbole.</td>
		<td>Le mode exact permet de sélectionner à un degré précis les symboles.</td>
	</tr>
   </tbody>
</table>

**Propriété:**
- La propriété <u>***RatingMode***  *(RatingMode)*</u> permet de choisir le mode du contrôle.

	Vous avez le choix entre 3 modes :
	- Standard (valeur par défaut)
	- Half
	- Exact

	<code><rating:Rating RatingMode="Exact"/></code>

## Lecture seule
<img src="https://i.ibb.co/Xx6W1LC/Rating-Read-Only.gif" alt="Rating-Read-Only">

Si vous activer le mode "Read only", l'utilisateur ne pourra pas modifier son vote. Cette propriété est pratique si vous voulez afficher en lecture seule la note.

**Propriété:**
- La propriété <u>***ReadOnly*** *(boolean)*</u> permet d'activer le mode lecture seule.
<code><rating:Rating ReadOnly="True"/></code>
*Valeur par défaut : "False"*

## Couleurs 
<img src="https://i.ibb.co/10ZBQ71/Rating-color-horizontal.png" alt="Rating-color-horizontal" >

Vous pouvez personnaliser la couleur des symboles sélectionnés et la couleur des symboles non sélectionnés.

**Propriétés:**
- La propriété <u>***SelectedStarColor*** *(Brush)*</u> permet de choisir la couleur des symboles qui ont été sélectionnés.
<code><rating:Rating SelectedStarColor="#FF00DCF"/></code>
*Valeur par défaut : "Red"*

- La propriété <u>***NormalStarColor*** *(Brush)*</u> permet de choisir la couleur des symboles non sélectionnés.
<code><rating:Rating NormalStarColor="#FFF1F1F1"/></code>
*Valeur par défaut : "Transparent"*


## Taille
<img src="https://i.ibb.co/vvH9TnZ/Rating-Smaller-Horizontal.png" alt="Rating-Smaller-Horizontal" >

Gérez facilement la taille des symboles.

**Propriétés:**
- La propriété <u>***StarSize*** *(double)*</u> permet de définir la taille des symboles.
<code><rating:Rating StarSize="55"/></code>
*Valeur par défaut : "40"*


## Nombre de symboles
<img src="https://i.ibb.co/LhF6t2X/Rating-stars-count.png" alt="Rating-stars-count">

 Vous pouvez choisir le nombre de symboles affichés.

**Propriétés:**
- La propriété <u>***StarsCount*** *(double)*</u> permet de définir le nombre de symboles affichés.
<code><rating:Rating StarsCount="3"/></code>
*Valeur par défaut : "5"*

## Taille des bordures
<img src="https://i.ibb.co/zrq6WJ0/Rating-border-size-horizontal.png" alt="Rating-border-size-horizontal">

La taille des bordure est personnalisable.

**Propriétés:**
- La propriété <u>***StarBorderSize*** *(double)*</u> permet de définir la taille de la bordure des symboles
<code><rating:Rating StarBorderSize="2"/></code>
*Valeur par défaut : "0.5"*

## Espace entre les symboles
<img src="https://i.ibb.co/6y2t6s2/Rating-Stars-spacing-horizontal.png" alt="Rating-Stars-spacing-horizontal">

Gérez facilement l'espace entre les symboles.

**Propriétés:**
- La propriété <u>***StarSpacing*** *(double)*</u> permet de gérer l'espace entre les symboles.
<code><rating:Rating StarSpacing="20"/></code>
*Valeur par défaut : "5"*

## Événements

- L'événement ***RateEvent*** se déclenche lorsque l'utilisateur note (clic sur un symbole).
	<code><rating:Rating RateEvent="MyRateEvent"/></code>
	
	L'événement appel une méthode dont la signature est la suivante :
	<code>void MyRateEvent(double rate, double percentRate)</code>
	
	* *rate* : Nombre de symboles (entier ou non) de sélectionnés 
	* *percentRate* : Pourcentage de symboles sélectionnés 
