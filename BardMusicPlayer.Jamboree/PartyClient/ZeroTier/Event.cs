/*
 * Copyright (c)2013-2021 ZeroTier, Inc.
 *
 * Use of this software is governed by the Business Source License included
 * in the license here: https://github.com/zerotier/libzt/blob/master/LICENSE.txt
 *
 * Change Date: 2026-01-01
 *
 * On the date above, in accordance with the Business Source License, use
 * of this software will be governed by version 2.0 of the Apache License.
 */
/****/

namespace BardMusicPlayer.Jamboree.PartyClient.ZeroTier
{
    internal class Event
    {
        public NodeInfo NodeInfo { get; set; }

        public NetworkInfo NetworkInfo { get; set; }

        public RouteInfo RouteInfo { get; set; }

        public PeerInfo PeerInfo { get; set; }

        public AddressInfo AddressInfo { get; set; }

        public int Code { get; set; }

        public string Name { get; set; }
    }
}