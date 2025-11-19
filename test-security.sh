#!/bin/bash

# Script de test de sécurité HTTPS
echo "🔒 Test de Sécurité HTTPS - Application Formation"
echo "=================================================="
echo ""

# Attendre que l'application démarre
echo "⏳ Attente du démarrage de l'application..."
sleep 3

echo "🔍 Test des en-têtes de sécurité HTTP..."
echo ""

# Test avec curl
echo "📡 Requête HTTPS vers https://localhost:7066"
echo "-------------------------------------------"
curl -I -k https://localhost:7066 2>/dev/null | grep -E "HTTP|strict-transport-security|x-frame-options|x-content-type-options|x-xss-protection|content-security-policy|referrer-policy|permissions-policy"

echo ""
echo "✅ Tests terminés !"
echo ""
echo "📋 En-têtes de sécurité attendus :"
echo "  ✓ Strict-Transport-Security (HSTS)"
echo "  ✓ X-Frame-Options: DENY"
echo "  ✓ X-Content-Type-Options: nosniff"
echo "  ✓ X-XSS-Protection: 1; mode=block"
echo "  ✓ Content-Security-Policy"
echo "  ✓ Referrer-Policy"
echo "  ✓ Permissions-Policy"
