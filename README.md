# Interactive Sand Sculpture VFX via OSC

![GIF1](https://github.com/user-attachments/assets/b0fb0641-0d67-403c-86d4-54b06b290d28)

![GIF2](https://github.com/user-attachments/assets/7fe7da03-4d73-4db8-9361-b566c518fbad)

スマートフォンのセンサーを利用し、リアルタイムに3D彫刻を崩壊・修復させるインタラクティブアート。
OSC通信を用いたフィジカルなデバイス連携と、VFX Graphによるパーティクル制御を組み合わせています。

[▶ Vimeoで見る](https://vimeo.com/1175425761?fl=tl&fe=ec)

## 概要

ZIG SIMアプリから送信されるデバイスの傾き情報（OSC）を受信し、画面内の胸像彫刻を砂のように崩壊させます。
地面と水平から垂直にデバイスを戻すと、崩れた砂が逆再生されるように元の彫刻の姿へと修復されます。

## 技術スタック

- Unity
- VFX Graph
- OSC (OSCJack)
- C#
- ZIGSIM (iOS App)

## セットアップと使用方法

### ZIGSIMの設定

* アプリを起動し、SETTINGSタブから以下を設定します。
* `Device UUID` : **`iphone`** (※Unity側の設定と一致させる)
* `IP Address` : 実行するPCのローカルIPアドレス
* `Port Number` : **`9000`**
* SENSORタブで、`Gravity`を有効にし、STARTを押す。
