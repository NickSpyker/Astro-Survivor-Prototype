# Gatling Enemy Setup Guide

## Problèmes corrigés / Fixed Issues

✅ **GatlingEnemy script mis à jour** - Utilise maintenant le ProjectilePoolManager
✅ **Système de tir fonctionnel** - Tire des projectiles vers le joueur
✅ **Integration avec le juice system** - Muzzle flash et effets visuels
✅ **Comportement intelligent** - Maintient sa distance et strafe

---

## Ce que j'ai changé / What I Changed

### GatlingEnemy.cs

**Avant:**
- Utilisait un `projectilePrefab` obsolète
- Code commenté pour `EnemyProjectile` qui n'existait pas
- Pas d'intégration avec le pooling system

**Maintenant:**
- ✅ Utilise `ProjectilePoolManager` pour spawner les projectiles
- ✅ Configuration via Inspector (damage, cooldown, pool name)
- ✅ Support pour `firePoint` (point de spawn du projectile)
- ✅ Muzzle flash effects intégrés
- ✅ Warnings clairs si ProjectilePoolManager manque

---

## Setup en Unity

### 1. ProjectilePoolManager (Si pas déjà fait)

**Créer le manager:**
1. Hierarchy → Create Empty → "ProjectilePoolManager"
2. Add Component → **ProjectilePoolManager** script

**Configurer les pools:**
```
ProjectilePoolManager Inspector:
├── Pools (Size: 2)
│   ├── [0] Player Bullets
│   │   ├── Pool Name: "PlayerBullet"
│   │   ├── Projectile Prefab: [Drag ton projectile player]
│   │   ├── Pool Size: 150
│   │   └── Expandable: ✓
│   └── [1] Enemy Bullets
│       ├── Pool Name: "EnemyBullet"
│       ├── Projectile Prefab: [Drag ton projectile enemy]
│       ├── Pool Size: 200
│       └── Expandable: ✓
```

**Important:** Le nom "EnemyBullet" doit correspondre exactement!

### 2. Créer le Prefab Enemy Projectile

Si tu n'as pas encore de projectile pour les ennemis:

1. **Créer le GameObject:**
   - Create → 3D Object → Sphere (ou Capsule)
   - Nom: "EnemyProjectile"
   - Scale: (0.2, 0.2, 0.2) ou similaire

2. **Ajouter les Components:**
   - Add Component → **Projectile** script
   - Add Component → **Sphere Collider** (Is Trigger = TRUE)
   - Add Component → **Rigidbody** (Is Kinematic = TRUE, Use Gravity = FALSE)

3. **Configurer Projectile:**
   ```
   Projectile Stats:
   - Speed: 30
   - Lifetime: 5
   - Damage: 5
   - Piercing: 0
   - Range: 100
   ```

4. **Visuels (Optionnel):**
   - Changer la couleur du material (rouge/orange pour ennemi)
   - Ajouter Trail Renderer
   - Ajouter une petite lumière

5. **Sauvegarder comme Prefab:**
   - Drag vers Project → Prefabs folder

### 3. Setup Gatling Enemy Prefab

**Créer/Modifier le prefab:**

1. **Ouvrir ou créer le Gatling Enemy prefab**

2. **Composants requis:**
   - ✓ **GatlingEnemy** script (hérite de Enemy)
   - ✓ **Rigidbody** (Use Gravity = FALSE)
   - ✓ **Collider** (pour collision avec player)
   - ✓ Tag: "Enemy"

3. **Créer Fire Point:**
   ```
   GatlingEnemy (Root)
   └── FirePoint (Empty GameObject)
       - Position: (0, 0, 1) ou devant l'ennemi
       - Rotation: (0, 0, 0)
   ```

4. **Configurer GatlingEnemy Component:**
   ```
   Enemy Data: [Assign EnemyData scriptable object]

   Weapon:
   - Attack Cooldown: 0.5 (tire toutes les 0.5s)
   - Keep Distance Range: 10 (garde 10 unités de distance)
   - Fire Point: [Drag le FirePoint child]
   - Projectile Pool Name: "EnemyBullet"
   - Projectile Damage: 5
   ```

5. **Visual (Optionnel):**
   - Ajouter un mesh/sprite différent des Ram enemies
   - Couleur différente (ex: bleu pour gatling, rouge pour ram)

### 4. Créer EnemyData pour Gatling

**Créer le Scriptable Object:**

1. Project → Right Click → Create → Scriptable Objects → **EnemyData**
2. Nom: "GatlingEnemyData" ou "HunterData"

3. **Configurer:**
   ```
   Enemy Name: "Hunter"
   Type: Hunter

   Stats:
   - Base HP: 30
   - Base Damage: 5
   - Base Speed: 8

   Behavior:
   - Behavior Type: RangedGatling
   - Attack Range: 20

   Visuals:
   - Prefab: [Drag ton GatlingEnemy prefab]
   ```

### 5. Ajouter aux Waves

**Option A: Via WaveData Scriptable Object**

1. Ouvre ton WaveData existant
2. Dans "Enemies" array:
   ```
   Enemies (Size: 2)
   ├── [0] Ram Enemies
   │   ├── Enemy Data: RamEnemyData
   │   ├── Count: 5
   │   └── Spawn Delay: 0.5
   └── [1] Gatling Enemies (NEW!)
       ├── Enemy Data: GatlingEnemyData
       ├── Count: 2
       └── Spawn Delay: 1.0
   ```

**Option B: Créer une nouvelle Wave avec Gatling**

1. Project → Right Click → Create → Scriptable Objects → **WaveData**
2. Nom: "Wave_Mixed" ou "Wave_Gatling"
3. Configurer avec mix de Ram + Gatling enemies

**Option C: Spawner manuellement pour tester**

Dans WaveManager, tu peux temporairement spawner directement:
```csharp
// Dans Start() du WaveManager pour tester
SpawnEnemy(gatlingEnemyData, 1);
```

---

## Testing Checklist

### Avant de lancer le jeu, vérifie:

- [ ] ProjectilePoolManager existe dans la scène
- [ ] Pool "EnemyBullet" est configuré avec le bon prefab
- [ ] Le prefab EnemyProjectile a:
  - [ ] Projectile script
  - [ ] Collider (Is Trigger = TRUE)
  - [ ] Rigidbody
- [ ] GatlingEnemy prefab a:
  - [ ] GatlingEnemy script
  - [ ] FirePoint child object
  - [ ] EnemyData assigné
  - [ ] Tag "Enemy"
- [ ] Au moins une Wave contient GatlingEnemyData

### Pendant le jeu:

✅ **Les gatling enemies doivent:**
- Apparaître dans les waves
- Maintenir une distance du joueur
- Tirer des projectiles toutes les 0.5s
- Strafer (bouger latéralement)
- Reculer si trop proche
- Avoir un muzzle flash visible

✅ **Les projectiles ennemis doivent:**
- Voler vers le joueur
- Faire des dégâts au joueur
- Disparaître après impact ou timeout
- Être rouges/orange (différents des projectiles player)

---

## Troubleshooting

### Les gatling enemies ne tirent pas?

**Check 1: ProjectilePoolManager**
```
Console error: "ProjectilePoolManager not found!"
→ Solution: Créer le GameObject avec ProjectilePoolManager script
```

**Check 2: Pool Name**
```
Console warning: "Pool EnemyBullet doesn't exist!"
→ Solution: Dans ProjectilePoolManager, vérifier que le pool name est exactement "EnemyBullet"
```

**Check 3: Player Reference**
```
→ Le player doit avoir le Tag "Player"
→ Vérifier dans Enemy.cs Start() que player est trouvé
```

**Check 4: Fire Point**
```
→ Si FirePoint n'est pas assigné, les projectiles spawneront devant l'ennemi
→ Pas critique mais mieux avec FirePoint
```

### Les projectiles ne font pas de dégâts au joueur?

**Check 1: Player a IDamageable**
```
→ Le Player doit avoir PlayerHealth component
→ PlayerHealth implémente IDamageable
```

**Check 2: Projectile Owner**
```
→ Dans Projectile.cs, vérifier HandleEnemyProjectileCollision()
→ Doit check le tag "Player"
```

**Check 3: Collision Layers**
```
→ Edit → Project Settings → Physics
→ Layer Collision Matrix
→ Vérifier que Enemy layer et Player layer peuvent collider
```

### Les gatling enemies ne spawn pas?

**Check 1: WaveData**
```
→ Ouvrir le WaveData dans Inspector
→ Vérifier que GatlingEnemyData est dans la liste
→ Count doit être > 0
```

**Check 2: EnemyData Prefab**
```
→ Ouvrir GatlingEnemyData ScriptableObject
→ Vérifier que le prefab field est assigné
→ Le prefab doit être celui avec GatlingEnemy script
```

**Check 3: WaveManager**
```
→ WaveManager doit être actif dans la scène
→ Zones array doit contenir au moins 1 zone
→ Cette zone doit avoir des waves
```

### Les projectiles traversent tout?

**Check: Collider settings**
```
EnemyProjectile prefab:
→ Collider: Is Trigger = TRUE
→ Rigidbody: Is Kinematic = TRUE
→ Layer: Default ou Projectile

Player:
→ Doit avoir un Collider
→ Layer doit pouvoir collider avec projectiles
```

---

## Customization Ideas

### Différents patterns de tir:

**Burst Fire:**
```csharp
// Dans GatlingEnemy, modifier Shoot()
for (int i = 0; i < 3; i++)
{
    // Spawn projectile
    yield return new WaitForSeconds(0.1f);
}
```

**Spread Shot:**
```csharp
// Tirer 3 projectiles en éventail
for (int i = -1; i <= 1; i++)
{
    Vector3 dir = Quaternion.Euler(0, i * 15f, 0) * direction;
    // Spawn avec dir
}
```

**Predictive Aim:**
```csharp
// Viser où le joueur va être
Vector3 playerVelocity = player.GetComponent<Rigidbody>().linearVelocity;
Vector3 futurePos = player.position + playerVelocity * 0.5f;
Vector3 direction = (futurePos - spawnPosition).normalized;
```

### Types de Gatling Enemies:

**Sniper (Long range, slow fire):**
```
- Attack Cooldown: 2.0
- Keep Distance Range: 20
- Projectile Damage: 15
- Projectile Speed: 50
```

**Minigun (Close range, rapid fire):**
```
- Attack Cooldown: 0.1
- Keep Distance Range: 5
- Projectile Damage: 2
- Projectile Speed: 40
```

**Shotgun (Close range, spread):**
```
- Attack Cooldown: 1.0
- Multiple projectiles per shot
- Keep Distance Range: 8
```

---

## Performance Tips

### Si beaucoup de gatling enemies:

1. **Augmenter Pool Size:**
   ```
   ProjectilePoolManager:
   - EnemyBullet Pool Size: 300-500
   - Expandable: TRUE (safety net)
   ```

2. **Limiter Spawn Rate:**
   ```
   WaveData:
   - Spawn Delay: 1.0+ (évite trop d'ennemis en même temps)
   ```

3. **Réduire Particle Effects:**
   ```
   GatlingEnemy:
   - Commenter la ligne muzzle flash si lag
   ```

4. **Optimiser Collision:**
   ```
   - Utiliser des Sphere Colliders (plus rapides)
   - Simplifier les collision layers
   ```

---

## Advanced: Different Enemy Weapons

Si tu veux créer d'autres types d'ennemis avec armes:

**Option 1: Réutiliser GatlingEnemy**
- Juste changer les valeurs dans l'Inspector
- Différents prefabs, même script

**Option 2: Utiliser EnemyWeapon Component**
- Ajouter EnemyWeapon.cs à un ennemi Ram
- Hybride melee + ranged

**Option 3: Créer de nouveaux scripts**
- RocketEnemy (projectiles explosifs)
- LaserEnemy (raycast instant hit)
- SniperEnemy (très long range)

---

## Files Modified

- ✅ **GatlingEnemy.cs** - Système de tir complet et fonctionnel
- ✅ **EnemyWeapon.cs** - Déjà existant (alternative au GatlingEnemy)
- ℹ️ **ProjectilePoolManager** - Doit avoir pool "EnemyBullet"

## Files You Need to Create/Configure

- 📝 **EnemyProjectile prefab** - Projectile pour les ennemis
- 📝 **GatlingEnemyData** - ScriptableObject pour stats
- 📝 **GatlingEnemy prefab** - Avec tous les components
- 📝 **WaveData** - Ajouter gatling enemies aux waves

---

## Quick Summary / Résumé Rapide

**Pour faire spawner et tirer les Gatling Enemies:**

1. ✅ J'ai corrigé le script GatlingEnemy.cs
2. 🔧 Tu dois créer le prefab EnemyProjectile avec Projectile component
3. 🔧 Configurer ProjectilePoolManager avec pool "EnemyBullet"
4. 🔧 Assigner le GatlingEnemy prefab dans EnemyData
5. 🔧 Ajouter GatlingEnemyData dans tes WaveData

**Le tir fonctionne maintenant!** Il te faut juste créer les assets dans Unity et les connecter.

Good luck! 🚀
