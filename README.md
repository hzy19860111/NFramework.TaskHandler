# NFramework.TaskHandler
基于消息队列的任务处理框架（待完成）

## 背景

ERP：
* 大数据量
* 批量操作
* 特定组织之间无关联，操作无影响
* 同一组织对单据并发零容忍

基于此背景

## 框架时序图

![框架时序图](NFramework.TaskHandler/blob/master/resource/nframeworksequencediagram.png)

## 框架处理流程



## 框架特点
* 可水平扩展
* 单队列无并发
* 开发方便
* 无特定依赖（目前只强依赖log4net，后面会修改）
* 提供Redis队列实现
* 路由机制（可自定义路由规则）


## 待完善
* 自定义Routing 
* TaskQueueCount、TaskQueueType以及RedisAppName 整合为Setting，每个单独任务 有单独的Setting
* 文档整理
