#!/bin/sh

ethdir=/home/eth

until [ -e $ethdir/data/geth.ipc ]
do
    sleep 5
done

geth attach --exec 'admin.nodeInfo.enode' $ethdir/data/geth.ipc  > $ethdir/enode