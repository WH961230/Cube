using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


namespace LazyPan {
    public class Behaviour_Auto_RobotHealth : Behaviour {
        private Flow_SceneB _flow;
        private Comp _ui;
        private FloatData _maxHealthData;//血量上限
        private FloatData _healthData;//血量
        private FloatData _beheadHealthRatio;
        private BoolData _chargyingEnergyData;//充能中
        private BoolData _invincibleData;
        private BoolData _burn;
        private BoolData _frost;
        private BoolData _frozen;
        private FloatData _autoAbsorbExperienceRatio;//自动吸收经验值概率
        private FloatData _burnTime;
        private FloatData _burnAttack;
        private FloatData _frostTime;
        private FloatData _frostSlowRatio;
        private StringData _beHitSound;
        private BoolData _antibodyBool;
        private FloatData _rebornData;
        private BoolData _deadBoomData;
        private Image _healthImg;
        private float burnDeploy;
        private float frostDeploy;
        private float frozenDeploy;
        private NavMeshAgent _navMeshAgent;
        private Transform _body;
        
        private Vector3 originalDestination;
        private Vector3 originalRespawnPosition;
        private float respawnDelay = 2f;

        public Behaviour_Auto_RobotHealth(Entity entity, string behaviourSign) : base(entity, behaviourSign) {
            Flo.Instance.GetFlow(out _flow);
            _ui = _flow.GetUI();

            _healthImg = Cond.Instance.Get<Image>(entity, LabelStr.HEALTH);
            
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.MAX, LabelStr.HEALTH), out _maxHealthData);
            Cond.Instance.GetData(entity, LabelStr.HEALTH, out _healthData);
            _healthData.Float = _maxHealthData.Float;

            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.BE, LabelStr.HIT, LabelStr.SOUND),
                out _beHitSound);
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), LabelStr.Assemble(LabelStr.BEHEAD, LabelStr.HEALTH, LabelStr.RATIO), out _beheadHealthRatio);
            Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(),
                LabelStr.Assemble(LabelStr.AUTO, LabelStr.ABSORB, LabelStr.EXPERIENCE, LabelStr.RATIO),
                out _autoAbsorbExperienceRatio);
            Cond.Instance.GetData(entity, LabelStr.BURN, out _burn);
            Cond.Instance.GetData(entity, LabelStr.FROST, out _frost);
            Cond.Instance.GetData(entity, LabelStr.FROZEN, out _frozen);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.BURN, LabelStr.TIME),
                out _burnTime);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.BURN, LabelStr.ATTACK),
                out _burnAttack);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.FROST, LabelStr.TIME),
                out _frostTime);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                LabelStr.Assemble(LabelStr.FROST, LabelStr.SLOW, LabelStr.RATIO),
                out _frostSlowRatio);
            Cond.Instance.GetData(entity, LabelStr.ANTIBODY, out _antibodyBool);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.REBORN, LabelStr.RATIO),
                out _rebornData);
            Cond.Instance.GetData(entity, LabelStr.Assemble(LabelStr.DEAD, LabelStr.BOOM),
                out _deadBoomData);

            _body = Cond.Instance.Get<Transform>(entity, LabelStr.BODY);
            _navMeshAgent = Cond.Instance.Get<NavMeshAgent>(entity, LabelStr.NAVMESHAGENT);

            MessageRegister.Instance.Reg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
            MessageRegister.Instance.Reg<int>(MessageCode.MsgBurnEntity, Burn);
            MessageRegister.Instance.Reg<int>(MessageCode.MsgFrostEntity, Frost);
            MessageRegister.Instance.Reg<int>(MessageCode.MsgBoomEntity, Boom);
            MessageRegister.Instance.Reg<int>(MessageCode.MsgFrozenEntity, Frozen);

            Game.instance.OnUpdateEvent.AddListener(OnUpdate);
        }

        public override void DelayedExecute() {
            
        }

        private void OnUpdate() {
            if (_healthImg != null) {
                _healthImg.fillAmount = _healthData.Float / _maxHealthData.Float;
            }

            OnBurnAndFrost();
        }

        //燃烧 燃烧时间完成后再次检测
        private void Burn(int entityId) {
            if (entity.ID == entityId) {
                if (_antibodyBool.Bool) {
                    return;
                }
                //燃烧几秒后 判断是否燃烧区域中
                burnDeploy = 1;

                //爆炸概率
                Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), LabelStr.Assemble(LabelStr.BOOM, LabelStr.RATIO), out FloatData boomRatioData);
                if (boomRatioData.Float != 0) {
                    float rand = Random.Range(0, 1);
                    if (rand <= boomRatioData.Float) {
                        Boom(entityId);
                    }
                }
            }
        }

        private void Boom(int entityId) {
            if (entity.ID == entityId) {
                if (_antibodyBool.Bool) {
                    return;
                }

                Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.BOOM, LabelStr.RANGE), out FloatData boomRangeData);
                Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.BOOM, LabelStr.ATTACK), out FloatData boomAttackData);
                        
                //激活触发器
                Vector3 robotBody = Cond.Instance.Get<Transform>(entity, LabelStr.BODY).position;
                Collider[] colliders = Physics.OverlapSphere(robotBody, boomRangeData.Float);
                foreach (var tmpCollider in colliders) {
                    if (EntityRegister.TryGetEntityByBodyPrefabID(tmpCollider.gameObject.GetInstanceID(), out Entity bodyEntity)) {
                        if (bodyEntity.ObjConfig.Type == "机器人") {
                            BeDamaged(bodyEntity.ID, boomAttackData.Float);
                        }
                    }
                }
            }
        }

        //冰霜
        private void Frost(int entityId) {
            if (entity.ID == entityId) {
                if (_antibodyBool.Bool) {
                    return;
                }

                //冰霜减速几秒 判断是否在减速区域中
                Cond.Instance.GetData(Cond.Instance.GetPlayerEntity(), LabelStr.Assemble(LabelStr.FROZEN, LabelStr.RATIO), out FloatData frozenRatioData);
                if (frozenRatioData.Float != 0) {
                    float rand = Random.Range(0, 1);
                    if (rand <= frozenRatioData.Float) {
                        Frozen(entityId);
                    }
                }

                if (frozenDeploy <= 0) {
                    frostDeploy = 1;
                }
            }
        }

        private void Frozen(int entityId) {
            if (entity.ID == entityId) {
                if (_antibodyBool.Bool) {
                    return;
                }

                Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(),
                    LabelStr.Assemble(LabelStr.FROZEN, LabelStr.TIME), out FloatData frozenData);
                frozenDeploy = frozenData.Float;
            }
        }

        private void OnBurnAndFrost() {
            if (burnDeploy > 0) {
                burnDeploy -= Time.deltaTime;
                Debug.LogFormat("角色:{0}受到灼烧伤害:{1}", entity.ID, _burnAttack.Float * Time.deltaTime);
                BeDamaged(entity.ID, _burnAttack.Float * Time.deltaTime);
                _burn.Bool = true;
            } else {
                _burn.Bool = false;
            }

            if (frostDeploy > 0) {
                frostDeploy -= Time.deltaTime;
                _frost.Bool = true;
            } else {
                _frost.Bool = false;
            }

            if (frozenDeploy > 0) {
                frozenDeploy -= Time.deltaTime;
                _frozen.Bool = true;
            } else {
                _frozen.Bool = false;
            }
        }

        private void BeDamaged(int entityId, float damageValue) {
            if (entity.ID == entityId) {
                if (_healthData.Float != 0) {
                    if (_healthData.Float > 0) {
                        //斩杀最小生命值低于系数的怪物
                        if (_healthData.Float < _maxHealthData.Float * _beheadHealthRatio.Float) {
                            damageValue = 9999;
                        }
                        _healthData.Float -= damageValue;
                        Sound.Instance.SoundPlay(_beHitSound.String, Vector3.zero, false, 2);
                    }

                    if (_healthData.Float <= 0) {
                        float rand = Random.Range(0.001f, 1);
                        if (_rebornData.Float != 0 && _rebornData.Float < rand) {
                            _healthData.Float = _maxHealthData.Float;
                            Respawn();
                            return;
                        }

                        if (_rebornData.Float == 0) {
                            _healthData.Float = 0;
                            Dead(damageValue != Mathf.Infinity);
                        }
                    }
                } 
            }
        }

        private void Respawn() {
            // 记录当前目标位置
            if (_navMeshAgent.enabled) {
                originalDestination = _navMeshAgent.destination;
                originalRespawnPosition = _navMeshAgent.transform.position;
            }

            // 停止 NavMeshAgent 控制
            _navMeshAgent.isStopped = true;
            _navMeshAgent.enabled = false;
            
            _body.gameObject.SetActive(false);

            _rebornData.Float = 0;

            // 开始复活过程
            ClockUtil.Instance.AlarmAfter(respawnDelay, RespawnAfter);
        }

        private void RespawnAfter() {
            _body.gameObject.SetActive(true);
            // 复活位置重置
            _body.position = originalRespawnPosition;

            // 恢复 NavMeshAgent 控制
            _navMeshAgent.enabled = true;
            _navMeshAgent.isStopped = false;

            // 重新设定目标位置（可根据需求调整）
            _navMeshAgent.SetDestination(originalDestination);
        }

        private void Dead(bool isDrop) {
            if (isDrop) {
                DeathDropExperience();
            }
            MessageRegister.Instance.Dis(MessageCode.MsgRobotDead, entity.ID);
            if (_deadBoomData.Bool) {
                MessageRegister.Instance.Dis(MessageCode.MsgBoomEntity, entity.ID);
            }
        }

        private void DeathDropExperience() {
            //掉落概率 1 - 100
            int randNum = Random.Range(0, 100);
            Cond.Instance.GetData(Cond.Instance.GetGlobalEntity(), LabelStr.Assemble(LabelStr.DROP, LabelStr.RATIO),
                out IntData intData);
            if (randNum <= intData.Int) {
                Entity drop = Obj.Instance.LoadEntity("Obj_可拾取_经验值");
                drop.SetBeginLocationInfo(new LocationInformationData() {
                    Position = Cond.Instance.Get<Transform>(entity, LabelStr.BODY).position
                });

                //有概率直接吸收
                float randAbsorb = Random.Range(0, 1);
                if (randAbsorb > 0 && randAbsorb < _autoAbsorbExperienceRatio.Float) {
                    if (BehaviourRegister.GetBehaviour(drop.ID,
                            out Behaviour_Trigger_PickUpExperiencePoint behaviour)) {
                        behaviour.Pick(Cond.Instance.GetPlayerEntity());
                    }
                }
            }
        }

        public override void Clear() {
            base.Clear();
            Game.instance.OnUpdateEvent.RemoveListener(OnUpdate);
            MessageRegister.Instance.UnReg<int, float>(MessageCode.MsgDamageRobot, BeDamaged);
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgBurnEntity, Burn);
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgFrostEntity, Frost);
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgBoomEntity, Boom);
            MessageRegister.Instance.UnReg<int>(MessageCode.MsgFrozenEntity, Frozen);
        }
    }
}