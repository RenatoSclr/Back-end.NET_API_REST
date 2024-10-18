# Findexium

Le produit est un logiciel d'entreprise en ligne destiné à faciliter les transactions pour les entreprises institutionnelles à revenu fixe, qu'elles soient acheteuses ou vendeuses. Ce logiciel a pour objectif d'aider ces entreprises à générer davantage de transactions en automatisant et en optimisant le processus de trading.

Le groupe développe également une application appelée PostTrades, qui vise à simplifier la communication et l'utilisation des informations post-transaction entre le front-office et le back-office. L'application permet d'améliorer le flux de travail après la transaction en assurant une gestion fluide des informations financières critiques.

## Instructions d'utilisation de l'application

### Prérequis:

*  **Cloner le repository** : Vous devez cloner le repository sur votre machine locale en utilisant la commande suivante dans votre terminal :

```bash
git clone https://github.com/RenatoSclr/Back-end.NET_API_REST.git
```
*  **Mettre à jour la base de données** : Une fois le projet cloné, vous devez mettre à jour la base de données. Les migrations sont déjà prêtes dans le projet. Exécutez la commande suivante pour synchroniser la base de données avec le modèle de données actuel :
```bash
dotnet ef database update
```
Cette commande applique toutes les migrations et prépare la base de données à l'utilisation.

## Démarrage de l'application
Après avoir mis à jour la base de données, vous pouvez démarrer l'application. Un utilisateur Administrateur est déjà créé dans la base de données à partir de cette étape. Vous pouvez utiliser ses identifiants de connexion pour commencer à utiliser l'application et accéder aux endpoints Admin, ou simplement vous enregistrer en tant qu'utilisateur lambda :

* **Nom d'utilisateur** : admin
* **Mot de passe** : Admin123!

## Utilisation des API
Vous pouvez tester les différentes API de l'application via **Swagger** ou **Postman**, qui vous permettra de manipuler les entités disponibles dans l'application, comme les utilisateurs, les Bids, les Trades etc... Pour tous les appels, une authentification est requise. Vous devrez utiliser le token JWT généré après connexion avec les identifiants administrateur ou un autre compte utilisateur.
