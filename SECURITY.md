# 🔒 Configuration de Sécurité HTTPS

## 🛡️ Mesures de Sécurité Implémentées

### 1. **HTTPS Obligatoire**
- ✅ Redirection automatique HTTP → HTTPS (308 Permanent Redirect)
- ✅ Port HTTPS : 7066
- ✅ Certificat de développement SSL/TLS

### 2. **HSTS (HTTP Strict Transport Security)**
- ✅ Force le navigateur à toujours utiliser HTTPS
- ✅ Durée : 365 jours
- ✅ Inclut les sous-domaines
- ✅ Preload activé

### 3. **Cookies Sécurisés**
- ✅ **HttpOnly** : Empêche l'accès JavaScript aux cookies (protection XSS)
- ✅ **Secure** : Cookies transmis uniquement via HTTPS
- ✅ **SameSite=Strict** : Protection CSRF avancée
- ✅ Expiration : 24 heures avec renouvellement glissant

### 4. **En-têtes de Sécurité HTTP**

#### X-Frame-Options: DENY
- Empêche l'intégration de la page dans une iframe
- Protection contre le clickjacking

#### X-Content-Type-Options: nosniff
- Empêche le navigateur de deviner le type MIME
- Protection contre les attaques de sniffing de contenu

#### X-XSS-Protection: 1; mode=block
- Active la protection XSS du navigateur
- Bloque la page en cas de détection d'attaque

#### Content-Security-Policy (CSP)
```
default-src 'self'
script-src 'self' 'unsafe-inline' 'unsafe-eval' cdnjs.cloudflare.com cdn.jsdelivr.net
style-src 'self' 'unsafe-inline' cdnjs.cloudflare.com fonts.googleapis.com
font-src 'self' cdnjs.cloudflare.com fonts.gstatic.com
img-src 'self' data: https:
connect-src 'self'
```
- Limite les sources de contenu autorisées
- Protection contre XSS et injection de contenu

#### Referrer-Policy: strict-origin-when-cross-origin
- Contrôle les informations de référence envoyées
- Protection de la vie privée

#### Permissions-Policy
- Désactive les fonctionnalités dangereuses :
  - Accéléromètre
  - Caméra
  - Géolocalisation
  - Gyroscope
  - Magnétomètre
  - Microphone
  - Paiements
  - USB

### 5. **Sécurité ASP.NET Identity**

#### Politique de Mots de Passe Renforcée
- ✅ Longueur minimale : **8 caractères**
- ✅ Au moins **1 chiffre**
- ✅ Au moins **1 minuscule**
- ✅ Au moins **1 majuscule**
- ✅ Au moins **1 caractère spécial**
- ✅ Au moins **4 caractères uniques**

#### Verrouillage de Compte
- ✅ **5 tentatives** de connexion échouées maximum
- ✅ Verrouillage pendant **15 minutes**
- ✅ Activé pour tous les nouveaux utilisateurs

#### Validation Email
- ✅ Emails uniques obligatoires
- ✅ Validation d'email (peut être activée en production)

### 6. **Protection CSRF**
- ✅ Jetons anti-forgery automatiques sur tous les formulaires
- ✅ Validation côté serveur
- ✅ Cookies SameSite=Strict

### 7. **Protection XSS**
- ✅ Encodage automatique des sorties Razor
- ✅ Content Security Policy
- ✅ En-têtes X-XSS-Protection

### 8. **Protection contre les Injections SQL**
- ✅ Entity Framework Core avec requêtes paramétrées
- ✅ Aucune requête SQL brute non sécurisée
- ✅ Validation des entrées utilisateur

## 🔧 Configuration Technique

### Certificat de Développement
```bash
# Vérifier le certificat
dotnet dev-certs https --check

# Régénérer si nécessaire
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

### Ports Utilisés
- **HTTPS** : 7066 (Principal)
- **HTTP** : 5178 (Redirige vers HTTPS)

## 🚀 Production

### Pour déployer en production, ajouter :

1. **Certificat SSL valide**
   - Obtenir un certificat d'une autorité de certification (Let's Encrypt, etc.)
   - Configurer dans le serveur web (Nginx, Apache, IIS)

2. **Variables d'environnement**
   ```bash
   ASPNETCORE_ENVIRONMENT=Production
   ASPNETCORE_URLS=https://+:443;http://+:80
   ```

3. **Configuration appsettings.Production.json**
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "server=prod-server;database=formationdb;..."
     },
     "Logging": {
       "LogLevel": {
         "Default": "Warning"
       }
     }
   }
   ```

4. **Reverse Proxy (Nginx)**
   ```nginx
   server {
       listen 80;
       server_name example.com;
       return 301 https://$server_name$request_uri;
   }
   
   server {
       listen 443 ssl http2;
       server_name example.com;
       
       ssl_certificate /path/to/cert.pem;
       ssl_certificate_key /path/to/key.pem;
       ssl_protocols TLSv1.2 TLSv1.3;
       ssl_ciphers HIGH:!aNULL:!MD5;
       
       location / {
           proxy_pass http://localhost:5000;
           proxy_http_version 1.1;
           proxy_set_header Upgrade $http_upgrade;
           proxy_set_header Connection keep-alive;
           proxy_set_header Host $host;
           proxy_cache_bypass $http_upgrade;
           proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
           proxy_set_header X-Forwarded-Proto $scheme;
       }
   }
   ```

## 📊 Tests de Sécurité

### Outils Recommandés
1. **Mozilla Observatory** : https://observatory.mozilla.org/
2. **SecurityHeaders.com** : https://securityheaders.com/
3. **SSL Labs** : https://www.ssllabs.com/ssltest/
4. **OWASP ZAP** : Scanner de vulnérabilités

### Vérification des En-têtes
```bash
curl -I https://localhost:7066
```

Devrait afficher :
```
HTTP/1.1 200 OK
Content-Type: text/html; charset=utf-8
Strict-Transport-Security: max-age=31536000; includeSubDomains; preload
X-Frame-Options: DENY
X-Content-Type-Options: nosniff
X-XSS-Protection: 1; mode=block
Content-Security-Policy: default-src 'self'; ...
Referrer-Policy: strict-origin-when-cross-origin
Permissions-Policy: accelerometer=(), camera=(), ...
```

## ⚠️ Avertissements

### Certificat de Développement
Le certificat auto-signé généré par `dotnet dev-certs` est **uniquement pour le développement**.

**En production** :
- ❌ N'utilisez JAMAIS un certificat auto-signé
- ✅ Utilisez un certificat d'une CA reconnue
- ✅ Utilisez Let's Encrypt (gratuit) ou un certificat commercial

### Données Sensibles
- ❌ Ne commitez JAMAIS de mots de passe ou clés dans Git
- ✅ Utilisez des variables d'environnement
- ✅ Utilisez Azure Key Vault ou similaire en production
- ✅ Rotation régulière des secrets

## 🔐 Bonnes Pratiques Supplémentaires

### 1. Rate Limiting
```csharp
// TODO: Ajouter AspNetCoreRateLimit
services.AddMemoryCache();
services.AddInMemoryRateLimiting();
```

### 2. Logging de Sécurité
- ✅ Logger toutes les tentatives de connexion échouées
- ✅ Logger les changements de permissions
- ✅ Logger les accès aux données sensibles

### 3. Mise à Jour Régulière
- ✅ Mettre à jour les packages NuGet mensuellement
- ✅ Surveiller les CVE (Common Vulnerabilities and Exposures)
- ✅ Appliquer les patches de sécurité rapidement

### 4. Validation des Entrées
- ✅ Valider TOUS les inputs utilisateur
- ✅ Utiliser les Data Annotations
- ✅ Limiter les tailles de fichiers upload
- ✅ Vérifier les extensions de fichiers

### 5. Principe du Moindre Privilège
- ✅ Utilisateur BDD avec droits minimums
- ✅ Rôles applicatifs bien définis
- ✅ Pas de compte admin par défaut en production

## 📚 Ressources

- [OWASP Top 10](https://owasp.org/www-project-top-ten/)
- [ASP.NET Core Security](https://docs.microsoft.com/aspnet/core/security/)
- [Mozilla Web Security Guidelines](https://infosec.mozilla.org/guidelines/web_security)
- [NIST Cybersecurity Framework](https://www.nist.gov/cyberframework)

## ✅ Checklist de Sécurité

- [x] HTTPS forcé
- [x] HSTS activé
- [x] Cookies sécurisés
- [x] Protection XSS
- [x] Protection CSRF
- [x] Protection Clickjacking
- [x] Content Security Policy
- [x] Mots de passe forts
- [x] Verrouillage de compte
- [x] Validation des entrées
- [x] Protection injection SQL
- [ ] Rate limiting
- [ ] Logging de sécurité avancé
- [ ] 2FA (Two-Factor Authentication)
- [ ] Certificat SSL en production
- [ ] Backup réguliers chiffrés
- [ ] Plan de réponse aux incidents

---

**Dernière mise à jour** : Novembre 2025  
**Niveau de sécurité** : 🟢 Élevé (Développement)  
**À faire pour production** : Rate limiting, 2FA, Certificat SSL valide
